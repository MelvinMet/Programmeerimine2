using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Authentication;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

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
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _db;
        public SaveUserCommandHandler(ApplicationDbContext db) => _db = db;

        public async Task<OperationResult> Handle(SaveUserCommand command, CancellationToken cancellationToken)
        {
            var user = command.Id == 0 ? new User() : await _db.Users.FindAsync(new object[] { command.Id }, cancellationToken);
            if (user == null) return new OperationResult().AddPropertyError("Id", $"Cannot find User with ID {command.Id}");
            if (command.Id == 0) _db.Users.Add(user);
            user.Name = command.Name;
            user.Phone = command.Phone;
            user.Email = command.Email;
            user.IsAdmin = command.IsAdmin;
            if (!string.IsNullOrWhiteSpace(command.Password)) user.Password = PasswordHelper.Hash(command.Password);
            await _db.SaveChangesAsync(cancellationToken);
            return new OperationResult();
        }
    }
    public class DeleteUserCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteUserCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteUserCommand, User>
    {
        public DeleteUserCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteUserCommand command) => command.Id;
    }
}
