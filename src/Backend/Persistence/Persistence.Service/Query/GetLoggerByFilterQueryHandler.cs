using Common.Domain.Entities;
using MediatR;
using Persistence.Repository;
using Persistence.Repository.Filters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Query
{
    public class GetLoggerByFilterQueryHandler : IRequestHandler<GetLoggerByFilterQuery, IEnumerable<Logger>>
    {
        private readonly IRepository<Logger, FilterLogger> _repositoryLogger;

        public GetLoggerByFilterQueryHandler(IRepository<Logger, FilterLogger> repositoryLogger)
        {
            _repositoryLogger = repositoryLogger;
        }

        public async Task<IEnumerable<Logger>> Handle(GetLoggerByFilterQuery request, CancellationToken cancellationToken)
        {
            return await _repositoryLogger.GetAll(new FilterLogger
                {
                    Start = request.Start,
                    End = request.End
                }, 
                cancellationToken);
        }
    }
}
