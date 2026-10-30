using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Bookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class BookingsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
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

            var query = new GetBookingQuery { Id = id.Value };
            var result = await _mediator.Send(query, cancellationToken);

            return Result(result);
        }

        [HttpGet("list/{page:int}/{pageSize:int}")]
        [HttpGet("list")]
        public async Task<IActionResult> List(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var query = new ListBookingsQuery { Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query, cancellationToken);

            return Result(result);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveBookingCommand command, CancellationToken cancellationToken = default)
            => Result(await _mediator.Send(command, cancellationToken));

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteBookingCommand command, CancellationToken cancellationToken = default)
            => Result(await _mediator.Send(command, cancellationToken));
    }
}
