using Common.Domain.Entities;
using Common.Pagination;
using MongoDB.Driver;
using Persistence.Repository;
using Persistence.Repository.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.MongoDB.Repository
{
    public class RepositoryQuery : IRepositoryQuery<Logger, FilterLogger>
    {
        private readonly ApplicationDbContext _context;

        public RepositoryQuery(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Logger> GetById(string id, CancellationToken cancellationToken)
        {
            return await _context.Loggers.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<DataCollection<Logger>> GetAll(FilterLogger filter, CancellationToken cancellationToken)
        {
            var query = _context.Loggers
                .Find(x => (filter.Start == null || x.Timestamp >= filter.Start) &&
                           (filter.End == null || x.Timestamp <= filter.End));

            return await query.GetPagedAsync<Logger>(filter.Page, filter.Take);
        }
    }
}
