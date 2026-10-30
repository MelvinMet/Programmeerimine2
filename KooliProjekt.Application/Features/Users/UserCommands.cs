using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
    }
    public class SaveUserCommandHandler : KooliProjekt.Application.Features.SaveEntityCommandHandler<SaveUserCommand, User>
    {
        public SaveUserCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(SaveUserCommand command) => command.Id;
    }
    public class DeleteUserCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteUserCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteUserCommand, User>
    {
        public DeleteUserCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteUserCommand command) => command.Id;
    }
}
