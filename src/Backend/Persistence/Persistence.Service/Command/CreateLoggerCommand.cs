using Common.Domain.Entities;
using MediatR;

namespace Persistence.Service.Command
{
    public class CreateLoggerCommand : IRequest<Logger>
    {
        public Logger Logger { get; set; }
    }
}
