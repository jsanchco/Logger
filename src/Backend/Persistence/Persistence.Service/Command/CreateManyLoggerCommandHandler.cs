using Common.Domain.Entities;
using MediatR;
using Persistence.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Command
{
    public class CreateManyLoggerCommandHandler : IRequestHandler<CreateManyLoggerCommand, int>
    {
        private readonly IRepositoryCommand<Logger> _repository;

        public CreateManyLoggerCommandHandler(IRepositoryCommand<Logger> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateManyLoggerCommand request, CancellationToken cancellationToken)
        {
            await _repository.AddManyAsync(request.Loggers, cancellationToken);

            return request.Loggers.Count;
        }
    }
}
