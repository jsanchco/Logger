using Common.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Repository;
using ServiceList;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Command
{
    public class CreateManyLoggerCommandHandler : IRequestHandler<CreateManyLoggerCommand, int>
    {
        private readonly IRepositoryCommand<Logger> _repository;
        private readonly ILogger<CreateManyLoggerCommandHandler> _logger;
        private readonly IServiceList<Logger> _serviceList;

        public CreateManyLoggerCommandHandler(
            IRepositoryCommand<Logger> repository,
            ILogger<CreateManyLoggerCommandHandler> logger,
            IServiceList<Logger> serviceList)
        {
            _repository = repository;
            _logger = logger;
            _serviceList = serviceList;
        }

        public async Task<int> Handle(CreateManyLoggerCommand request, CancellationToken cancellationToken)
        {
            await _repository.AddManyAsync(request.Loggers, cancellationToken);

            _serviceList.RemoveAllItems();

            return request.Loggers.Count;
        }
    }
}
