using Common.Domain.Entities;
using Common.Pagination;
using MediatR;
using System;

namespace Persistence.Service.Query
{
    public class GetLoggerByFilterQuery : IRequest<DataCollection<Logger>>
    {
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 10;

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
