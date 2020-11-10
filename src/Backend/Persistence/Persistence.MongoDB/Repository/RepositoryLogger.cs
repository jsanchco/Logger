using Common.Domain.Entities;
using MongoDB.Driver;
using Persistence.Repository;
using Persistence.Repository.Filters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.MongoDB.Repository
{
    public class RepositoryLogger : IRepository<Logger, FilterLogger>
    {
        private readonly ApplicationDbContext _context;

        public RepositoryLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Logger> GetById(string id, CancellationToken cancellationToken)
        {
            return await _context.Loggers.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Logger>> GetAll(FilterLogger filter, CancellationToken cancellationToken)
        {
            return await _context.Loggers
                .Find(x => (filter.Start == null || x.Timestamp >= filter.Start) &&
                           (filter.End == null || x.Timestamp <= filter.End))
                .ToListAsync();
        }

        public async Task<Logger> AddAsync(Logger entity, CancellationToken cancellationToken)
        {
            await _context.Loggers.InsertOneAsync(entity);

            return entity;
        }

        public Task<Logger> UpdateAsync(Logger entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddManyAsync(IEnumerable<Logger> entities, CancellationToken cancellationToken)
        {
            await _context.Loggers.InsertManyAsync(entities);
        }
    }
}
