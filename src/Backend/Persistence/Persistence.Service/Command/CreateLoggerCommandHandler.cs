using Common.Domain.Entities;
using MediatR;
using Persistence.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Command
{
    public class CreateLoggerCommandHandler : IRequestHandler<CreateLoggerCommand, Logger>
    {
        private readonly IRepositoryCommand<Logger> _repository;

        public CreateLoggerCommandHandler(IRepositoryCommand<Logger> repository)
        {
            _repository = repository;
        }

        public async Task<Logger> Handle(CreateLoggerCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddAsync(request.Logger, cancellationToken);
        }
    }
}
