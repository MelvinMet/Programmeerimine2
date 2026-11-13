using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Cars
{
    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, OperationResult>
    {
        private readonly ApplicationDbContext _db; private readonly IFileClient _files;
        public AddFileCommandHandler(ApplicationDbContext db, IFileClient files) { _db = db; _files = files; }
        public async Task<OperationResult> Handle(AddFileCommand request, CancellationToken ct)
        {
            if (!await _db.Cars.AnyAsync(x => x.Id == request.Id, ct)) return new OperationResult().AddPropertyError("Id", "Car does not exist.");
            await _files.Save(request.FileName, Store(request.Id), request.InputStream, ct); return new OperationResult();
        }
        internal static string Store(int id) => Path.Combine(FileStore.CarPhotos, id.ToString());
    }
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, OperationResult>
    {
        private readonly ApplicationDbContext _db; private readonly IFileClient _files;
        public DeleteFileCommandHandler(ApplicationDbContext db, IFileClient files) { _db = db; _files = files; }
        public async Task<OperationResult> Handle(DeleteFileCommand request, CancellationToken ct)
        {
            if (!await _db.Cars.AnyAsync(x => x.Id == request.Id, ct)) return new OperationResult().AddPropertyError("Id", "Car does not exist.");
            await _files.Delete(request.FileName, AddFileCommandHandler.Store(request.Id), ct); return new OperationResult();
        }
    }
    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, OperationResult<Stream>>
    {
        private readonly ApplicationDbContext _db; private readonly IFileClient _files;
        public GetFileQueryHandler(ApplicationDbContext db, IFileClient files) { _db = db; _files = files; }
        public async Task<OperationResult<Stream>> Handle(GetFileQuery request, CancellationToken ct)
        {
            var result = new OperationResult<Stream>();
            if (await _db.Cars.AnyAsync(x => x.Id == request.Id, ct)) result.Value = await _files.Get(request.FileName, AddFileCommandHandler.Store(request.Id), ct);
            return result;
        }
    }
    public class ListFilesQueryHandler : IRequestHandler<ListFilesQuery, OperationResult<System.Collections.Generic.IReadOnlyList<string>>>
    {
        private readonly ApplicationDbContext _db; private readonly IFileClient _files;
        public ListFilesQueryHandler(ApplicationDbContext db, IFileClient files) { _db = db; _files = files; }
        public async Task<OperationResult<System.Collections.Generic.IReadOnlyList<string>>> Handle(ListFilesQuery request, CancellationToken ct)
        {
            var result = new OperationResult<System.Collections.Generic.IReadOnlyList<string>>();
            if (await _db.Cars.AnyAsync(x => x.Id == request.Id, ct)) result.Value = await _files.List(AddFileCommandHandler.Store(request.Id), ct);
            return result;
        }
    }
}
