using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public interface IFileClient
    {
        Task Save(string fileName, string fileStore, Stream inputStream, CancellationToken cancellationToken = default);
        Task Delete(string fileName, string fileStore, CancellationToken cancellationToken = default);
        Task<Stream> Get(string fileName, string fileStore, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<string>> List(string fileStore, CancellationToken cancellationToken = default);
    }
}
