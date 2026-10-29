using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.CarModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class CarModelsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CarModelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get/{id?}")]
        public async Task<IActionResult> Get(int? id, CancellationToken cancellationToken = default)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return NotFound();
            }

            var query = new GetCarModelQuery { Id = id.Value };
            var result = await _mediator.Send(query, cancellationToken);

            return Result(result);
        }

        [HttpGet("list/{page:int}/{pageSize:int}")]
        [HttpGet("list")]
        public async Task<IActionResult> List(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var query = new ListCarModelsQuery { Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query, cancellationToken);

            return Result(result);
        }
    }
}
