using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Cars
{
    public class DeleteFileCommand : IRequest<OperationResult> { public int Id { get; set; } public string FileName { get; set; } }
}
