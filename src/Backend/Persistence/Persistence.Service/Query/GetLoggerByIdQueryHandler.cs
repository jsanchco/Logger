using Common.Domain.Entities;
using MediatR;
using Persistence.Repository;
using Persistence.Repository.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Query
{
    public class GetLoggerByIdQueryHandler : IRequestHandler<GetLoggerByIdQuery, Logger>
    {
        private readonly IRepository<Logger, FilterLogger> _repositoryLogger;

        public GetLoggerByIdQueryHandler(IRepository<Logger, FilterLogger> repositoryLogger)
        {
            _repositoryLogger = repositoryLogger;
        }

        public async Task<Logger> Handle(GetLoggerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repositoryLogger.GetById(request.Id, cancellationToken);
        }
    }
}
