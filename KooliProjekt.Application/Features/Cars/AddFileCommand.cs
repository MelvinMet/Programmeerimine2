using System.IO;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Cars
{
    public class AddFileCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public Stream InputStream { get; set; }
    }
}
