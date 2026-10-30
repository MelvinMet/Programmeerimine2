using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class SaveCarManufacturerCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SaveCarManufacturerCommandHandler : KooliProjekt.Application.Features.SaveEntityCommandHandler<SaveCarManufacturerCommand, CarManufacturer>
    {
        public SaveCarManufacturerCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(SaveCarManufacturerCommand command) => command.Id;
    }
    public class DeleteCarManufacturerCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteCarManufacturerCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteCarManufacturerCommand, CarManufacturer>
    {
        public DeleteCarManufacturerCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteCarManufacturerCommand command) => command.Id;
    }
}
