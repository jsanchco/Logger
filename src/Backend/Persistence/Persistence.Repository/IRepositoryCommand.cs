using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IRepositoryCommand<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task AddManyAsync(List<TEntity> entities, CancellationToken cancellationToken);
    }
}
