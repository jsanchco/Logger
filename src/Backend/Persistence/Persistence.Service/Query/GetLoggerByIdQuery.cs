using Common.Domain.Entities;
using MediatR;

namespace Persistence.Service.Query
{
    public class GetLoggerByIdQuery : IRequest<Logger>
    {
        public string Id { get; set; }
    }
}
