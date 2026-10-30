using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Cars
{
    public class SaveCarCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string RegistrationNo { get; set; }
        public int CarModelId { get; set; }
    }
    public class SaveCarCommandHandler : KooliProjekt.Application.Features.SaveEntityCommandHandler<SaveCarCommand, Car>
    {
        public SaveCarCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(SaveCarCommand command) => command.Id;
    }
    public class DeleteCarCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteCarCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteCarCommand, Car>
    {
        public DeleteCarCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteCarCommand command) => command.Id;
    }
}
