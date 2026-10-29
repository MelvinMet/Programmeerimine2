using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class ListInvoicesQueryHandler : IRequestHandler<ListInvoicesQuery, OperationResult<PagedResult<InvoiceListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListInvoicesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceListItemDto>>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<InvoiceListItemDto>>();
            result.Value = await _dbContext.Invoices
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new InvoiceListItemDto
                {
                    Id = item.Id,
                    InvoiceDate = item.InvoiceDate,
                    DueDate = item.DueDate,
                    UserName = item.User.Name,
                    IsPaid = item.IsPaid,
                    Total = item.InvoiceLines.Sum(line => line.Total)
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return result;
        }
    }
}
