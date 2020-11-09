using Common.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace Persistence.Service.Query
{
    public class GetLoggerByFilterQuery : IRequest<IEnumerable<Logger>>
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
