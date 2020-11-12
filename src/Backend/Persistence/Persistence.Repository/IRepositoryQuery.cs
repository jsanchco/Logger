using Common.Pagination;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IRepositoryQuery<TEntity, TFilter> where TEntity : class, new()
    {
        Task<TEntity> GetById(string id, CancellationToken cancellationToken);
        Task<DataCollection<TEntity>> GetAll(TFilter filter, CancellationToken cancellationToken);
    }
}
