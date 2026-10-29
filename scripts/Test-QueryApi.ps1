param([string]$BaseUrl = 'http://localhost:5086')

$ErrorActionPreference = 'Stop'
$script:requestCount = 0

function Assert-That($Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

function Get-Api([string]$Path, [int]$ExpectedStatus = 200) {
    $response = Invoke-WebRequest "$BaseUrl/api/$Path" -Headers @{ Accept = 'application/json' } -SkipHttpErrorCheck
    $script:requestCount++
    Assert-That ($response.StatusCode -eq $ExpectedStatus) "$Path : expected $ExpectedStatus, got $($response.StatusCode): $($response.Content)"
    if ($ExpectedStatus -ne 200) { return }
    Assert-That ($response.Headers['Content-Type'] -match 'application/json') "$Path : expected application/json"
    $body = $response.Content | ConvertFrom-Json
    Assert-That ($body -isnot [string]) "$Path : JSON must be an object, not a serialized JSON string"
    Assert-That ($null -ne $body.value -and -not $body.hasErrors) "$Path : missing OperationResult.Value or unexpected errors"
    Assert-That ($response.Content -notmatch '"(password|isAdmin)"\s*:') "$Path : private user fields were exposed"
    return $body.value
}

$displayFields = @{
    Users = 'name', 'email'
    Cars = 'registrationNo', 'modelName', 'manufacturerName'
    Bookings = 'registrationNo', 'userName'
    Invoices = 'userName'
    InvoiceLines = 'lineItem', 'unit'
    CarModels = 'name', 'manufacturerName'
    CarManufacturers = 'name'
}

foreach ($feature in 'Users', 'Cars', 'Bookings', 'Invoices', 'InvoiceLines', 'CarModels', 'CarManufacturers') {
    $first = Get-Api "$feature/list/1/10"
    Assert-That ($first.currentPage -eq 1 -and $first.pageSize -eq 10) "$feature : incorrect paging metadata"
    Assert-That ($first.rowCount -gt 10) "$feature : seed data must contain more than 10 records for this smoke test"
    Assert-That ($first.pageCount -eq [math]::Ceiling($first.rowCount / 10.0)) "$feature : incorrect page count"
    Assert-That ($first.results.Count -eq 10) "$feature : expected 10 records on first page"
    $firstIds = @($first.results.id)
    Assert-That (($firstIds -join ',') -eq (($firstIds | Sort-Object) -join ',')) "$feature : first page must be ordered by ID"
    foreach ($field in $displayFields[$feature]) {
        Assert-That (-not [string]::IsNullOrWhiteSpace($first.results[0].$field)) "$feature : missing display field $field"
    }

    $detail = Get-Api "$feature/get/$($firstIds[0])"
    Assert-That ($detail.id -eq $firstIds[0]) "$feature : Get returned the wrong record"
    foreach ($field in $displayFields[$feature]) {
        Assert-That ($detail.$field -eq $first.results[0].$field) "$feature : Get/List disagree on $field"
    }
    switch ($feature) {
        Users { Assert-That (-not [string]::IsNullOrWhiteSpace($detail.phone)) 'User detail needs a phone number' }
        Cars { Assert-That ($detail.carModelId -gt 0 -and $detail.kmRate -gt 0 -and $detail.hourlyRate -gt 0) 'Car detail needs model ID and rates' }
        Bookings { Assert-That ($detail.carId -gt 0 -and $detail.userId -gt 0 -and $detail.finishKm -ge $detail.startKm) 'Booking detail needs valid references and mileage' }
        Invoices { Assert-That ($null -ne $detail.lines -and $detail.userId -gt 0 -and $detail.userEmail) 'Invoice detail needs lines and customer details' }
        InvoiceLines { Assert-That ($detail.registrationNo -and $detail.bookingStarted -and $detail.bookingFinished) 'Invoice line detail needs booking information' }
        CarModels { Assert-That ($detail.carManufacturerId -gt 0 -and $detail.kmRate -gt 0 -and $detail.carCount -gt 0) 'Model detail needs manufacturer, rates and car count' }
        CarManufacturers { Assert-That ($detail.models.Count -gt 0 -and $detail.models[0].name) 'Manufacturer detail needs named models' }
    }

    $second = Get-Api "$feature/list/2/10"
    Assert-That ($second.currentPage -eq 2 -and $second.rowCount -eq $first.rowCount) "$feature : second page metadata mismatch"
    Assert-That ($second.results.Count -eq 10 -and $second.results[0].id -gt $firstIds[-1]) "$feature : second page overlaps the first"
    $last = Get-Api "$feature/list/$($first.pageCount)/10"
    Assert-That ($last.results.Count -eq ($first.rowCount - 10 * ($first.pageCount - 1))) "$feature : incorrect last page size"
    $empty = Get-Api "$feature/list/$($first.pageCount + 1)/10"
    Assert-That ($empty.results.Count -eq 0 -and $empty.rowCount -eq $first.rowCount) "$feature : page after last must return an empty list and retain row count"
    $extreme = Get-Api "$feature/list/2147483647/2147483647"
    Assert-That ($extreme.results.Count -eq 0) "$feature : large page values must not overflow"

    foreach ($path in 'list', 'list/0/0', 'list/-1/-5') {
        $defaults = Get-Api "$feature/$path"
        Assert-That ($defaults.currentPage -eq 1 -and $defaults.pageSize -eq 10 -and $defaults.results.Count -eq 10) "$feature/$path : expected default first page with size 10"
    }
    foreach ($id in '2147483647', '0', '-1', '') { Get-Api "$feature/get/$id" 404 }
    Write-Output "$feature : DTO, OperationResult, paging, defaults and 404 checks passed ($($first.rowCount) rows)."
}

Write-Output "Passed $script:requestCount HTTP checks."
