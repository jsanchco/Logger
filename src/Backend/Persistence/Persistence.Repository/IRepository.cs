using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IRepository<TEntity, TFilter> where TEntity : class, new()
    {
        Task<TEntity> GetById(string id, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAll(TFilter filter, CancellationToken cancellationToken);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
