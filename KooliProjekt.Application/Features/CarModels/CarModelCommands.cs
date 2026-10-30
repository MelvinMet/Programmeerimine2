using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.CarModels
{
    public class SaveCarModelCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarManufacturerId { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
    }
    public class SaveCarModelCommandHandler : KooliProjekt.Application.Features.SaveEntityCommandHandler<SaveCarModelCommand, CarModel>
    {
        public SaveCarModelCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(SaveCarModelCommand command) => command.Id;
    }
    public class DeleteCarModelCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteCarModelCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteCarModelCommand, CarModel>
    {
        public DeleteCarModelCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteCarModelCommand command) => command.Id;
    }
}
