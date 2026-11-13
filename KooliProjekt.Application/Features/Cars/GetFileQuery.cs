using System.IO;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Cars
{
    public class GetFileQuery : IRequest<OperationResult<Stream>> { public int Id { get; set; } public string FileName { get; set; } }
}
