using Common.Domain.Entities;
using MediatR;
using Persistence.Repository;
using Persistence.Repository.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Command
{
    public class CreateManyLoggerCommandHandler : IRequestHandler<CreateManyLoggerCommand, int>
    {
        private readonly IRepository<Logger, FilterLogger> _repositoryLogger;

        public CreateManyLoggerCommandHandler(IRepository<Logger, FilterLogger> repositoryLogger)
        {
            _repositoryLogger = repositoryLogger;
        }

        public async Task<int> Handle(CreateManyLoggerCommand request, CancellationToken cancellationToken)
        {
            await _repositoryLogger.AddManyAsync(request.Loggers, cancellationToken);

            return request.Loggers.Count;
        }
    }
}
