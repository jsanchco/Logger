using Common.Domain.Entities;
using Common.Pagination;
using MediatR;
using Persistence.Repository;
using Persistence.Repository.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Service.Query
{
    public class GetLoggerByFilterQueryHandler : IRequestHandler<GetLoggerByFilterQuery, DataCollection<Logger>>
    {
        private readonly IRepositoryQuery<Logger, FilterLogger> _repository;

        public GetLoggerByFilterQueryHandler(IRepositoryQuery<Logger, FilterLogger> repository)
        {
            _repository = repository;
        }

        public async Task<DataCollection<Logger>> Handle(GetLoggerByFilterQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAll(new FilterLogger
                {
                    Page = request.Page,
                    Take = request.Take,
                    Start = request.Start,
                    End = request.End
                }, 
                cancellationToken);
        }
    }
}
