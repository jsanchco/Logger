using Common.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Persistence.Service.Command
{
    public class CreateManyLoggerCommand : IRequest
    {
        public List<Logger> Loggers { get; set; }
    }
}
