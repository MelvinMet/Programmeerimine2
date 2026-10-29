using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class GetInvoiceLineQueryHandler : IRequestHandler<GetInvoiceLineQuery, OperationResult<InvoiceLineDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceLineQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceLineDto>> Handle(GetInvoiceLineQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<InvoiceLineDto>();
            result.Value = await _dbContext.InvoiceLines
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new InvoiceLineDto
                {
                    Id = item.Id,
                    LineItem = item.LineItem,
                    Unit = item.Unit,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    BookingId = item.BookingId,
                    RegistrationNo = item.Booking.Car.RegistrationNo,
                    BookingStarted = item.Booking.Started,
                    BookingFinished = item.Booking.Finished
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
