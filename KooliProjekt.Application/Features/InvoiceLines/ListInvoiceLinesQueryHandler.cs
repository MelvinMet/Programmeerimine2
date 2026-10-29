using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class ListInvoiceLinesQueryHandler : IRequestHandler<ListInvoiceLinesQuery, OperationResult<PagedResult<InvoiceLineListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListInvoiceLinesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceLineListItemDto>>> Handle(ListInvoiceLinesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<InvoiceLineListItemDto>>();
            result.Value = await _dbContext.InvoiceLines
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new InvoiceLineListItemDto
                {
                    Id = item.Id,
                    LineItem = item.LineItem,
                    Unit = item.Unit,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    BookingId = item.BookingId
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return result;
        }
    }
}
