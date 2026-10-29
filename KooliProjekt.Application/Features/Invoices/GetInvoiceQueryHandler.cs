using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<InvoiceDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceDto>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<InvoiceDto>();
            result.Value = await _dbContext.Invoices
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new InvoiceDto
                {
                    Id = item.Id,
                    InvoiceDate = item.InvoiceDate,
                    DueDate = item.DueDate,
                    UserName = item.User.Name,
                    IsPaid = item.IsPaid,
                    Total = item.InvoiceLines.Sum(line => line.Total),
                    UserId = item.UserId,
                    UserEmail = item.User.Email,
                    Lines = item.InvoiceLines.OrderBy(line => line.Id).Select(line => new InvoiceLineListItemDto
                    {
                        Id = line.Id,
                        LineItem = line.LineItem,
                        Unit = line.Unit,
                        Price = line.Price,
                        Quantity = line.Quantity,
                        Total = line.Total,
                        BookingId = line.BookingId
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
