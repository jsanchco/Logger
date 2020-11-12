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
        private readonly IRepositoryQuery<Logger, FilterLogger> _repository;

        public GetLoggerByIdQueryHandler(IRepositoryQuery<Logger, FilterLogger> repository)
        {
            _repository = repository;
        }

        public async Task<Logger> Handle(GetLoggerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetById(request.Id, cancellationToken);
        }
    }
}
