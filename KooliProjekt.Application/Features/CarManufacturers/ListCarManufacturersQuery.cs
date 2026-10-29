using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class ListCarManufacturersQuery : IRequest<OperationResult<PagedResult<CarManufacturerListItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
