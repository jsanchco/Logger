using Common.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Persistence.Service.Command
{
    public class CreateManyLoggerCommand : IRequest<int>
    {
        public List<Logger> Loggers { get; set; }
    }
}
