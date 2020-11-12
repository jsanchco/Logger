using Common.Domain.Entities;
using Persistence.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.MongoDB.Repository
{
    public class RepositoryCommand : IRepositoryCommand<Logger>
    {
        private readonly ApplicationDbContext _context;

        public RepositoryCommand(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Logger> AddAsync(Logger entity, CancellationToken cancellationToken)
        {
            await _context.Loggers.InsertOneAsync(entity);

            return entity;
        }

        public async Task AddManyAsync(List<Logger> entities, CancellationToken cancellationToken)
        {
            await _context.Loggers.InsertManyAsync(entities);
        }
    }
}
