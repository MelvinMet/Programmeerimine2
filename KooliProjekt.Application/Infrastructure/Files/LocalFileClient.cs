using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public class LocalFileClient : IFileClient
    {
        private readonly string _filesRoot;

        public LocalFileClient(string filesRoot)
        {
            if (string.IsNullOrWhiteSpace(filesRoot)) throw new ArgumentException("FilesRoot is required.", nameof(filesRoot));
            _filesRoot = Path.GetFullPath(filesRoot);
        }

        public async Task Save(string fileName, string fileStore, Stream inputStream, CancellationToken cancellationToken = default)
        {
            var path = GetPath(fileName, fileStore);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            await inputStream.CopyToAsync(output, cancellationToken);
        }

        public Task Delete(string fileName, string fileStore, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var path = GetPath(fileName, fileStore);
            if (File.Exists(path)) File.Delete(path);
            return Task.CompletedTask;
        }

        public Task<Stream> Get(string fileName, string fileStore, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var path = GetPath(fileName, fileStore);
            Stream stream = File.Exists(path) ? File.OpenRead(path) : null;
            return Task.FromResult(stream);
        }

        public Task<IReadOnlyList<string>> List(string fileStore, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var folder = GetPath("placeholder", fileStore);
            var result = (IReadOnlyList<string>)(Directory.Exists(Path.GetDirectoryName(folder))
                ? Directory.GetFiles(Path.GetDirectoryName(folder)).Select(Path.GetFileName).ToArray()
                : Array.Empty<string>());
            return Task.FromResult(result);
        }

        private string GetPath(string fileName, string fileStore)
        {
            if (string.IsNullOrWhiteSpace(fileStore) || fileStore.Contains("..") || Path.IsPathRooted(fileStore)) throw new ArgumentException("Invalid file store.", nameof(fileStore));
            if (string.IsNullOrWhiteSpace(fileName) || fileName != Path.GetFileName(fileName) || fileName.Contains("..")) throw new ArgumentException("Invalid file name.", nameof(fileName));
            var root = Path.GetFullPath(Path.Combine(_filesRoot, fileStore));
            if (!root.StartsWith(_filesRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("Invalid file store.", nameof(fileStore));
            return Path.Combine(root, fileName);
        }
    }
}
