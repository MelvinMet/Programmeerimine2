using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class GetCarManufacturerQueryHandler : IRequestHandler<GetCarManufacturerQuery, OperationResult<CarManufacturerDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCarManufacturerQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<CarManufacturerDto>> Handle(GetCarManufacturerQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<CarManufacturerDto>();
            result.Value = await _dbContext.CarManufacturers
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new CarManufacturerDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Models = item.CarModels.OrderBy(model => model.Id).Select(model => new CarModelListItemDto
                    {
                        Id = model.Id,
                        Name = model.Name,
                        ManufacturerName = item.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
