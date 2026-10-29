using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.CarModels
{
    public class ListCarModelsQuery : IRequest<OperationResult<PagedResult<CarModelListItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
