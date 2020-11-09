using Common.Domain.Entities;
using MediatR;
using Persistence.Repository;
using Persistence.Repository.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Command
{
    public class CreateLoggerCommandHandler : IRequestHandler<CreateLoggerCommand, Logger>
    {
        private readonly IRepository<Logger, FilterLogger> _repositoryLogger;

        public CreateLoggerCommandHandler(IRepository<Logger, FilterLogger> repositoryLogger)
        {
            _repositoryLogger = repositoryLogger;
        }

        public async Task<Logger> Handle(CreateLoggerCommand request, CancellationToken cancellationToken)
        {
            return await _repositoryLogger.AddAsync(request.Logger, cancellationToken);
        }
    }
}
