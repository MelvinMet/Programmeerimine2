using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features
{
    public abstract class SaveEntityCommandHandler<TCommand, TEntity> : IRequestHandler<TCommand, OperationResult>
        where TCommand : IRequest<OperationResult>
        where TEntity : class, new()
    {
        private readonly ApplicationDbContext _dbContext;

        protected SaveEntityCommandHandler(ApplicationDbContext dbContext) => _dbContext = dbContext;

        protected abstract int GetId(TCommand command);

        public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var id = GetId(request);
            var entity = id == 0 ? new TEntity() : await _dbContext.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
            if (entity == null)
            {
                return new OperationResult().AddPropertyError("Id", $"Cannot find {typeof(TEntity).Name} with ID {id}");
            }

            if (id == 0)
            {
                _dbContext.Set<TEntity>().Add(entity);
            }

            _dbContext.Entry(entity).CurrentValues.SetValues(request);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new OperationResult();
        }
    }

    public abstract class DeleteEntityCommandHandler<TCommand, TEntity> : IRequestHandler<TCommand, OperationResult>
        where TCommand : IRequest<OperationResult>
        where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;

        protected DeleteEntityCommandHandler(ApplicationDbContext dbContext) => _dbContext = dbContext;

        protected abstract int GetId(TCommand command);

        public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Set<TEntity>()
                .Where(entity => EF.Property<int>(entity, "Id") == GetId(request))
                .ExecuteDeleteAsync(cancellationToken);
            return new OperationResult();
        }
    }
}
