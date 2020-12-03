using Common.EventBus.Commands;
using Common.EventBus.EventBus;
using Common.EventBus.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Common.EventBus
{
    public class AzureEventBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AzureEventBus(
            IMediator mediator,
            IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Publish<T>(T @event) where T : Event
        {
            throw new System.NotImplementedException();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler
        {
            throw new System.NotImplementedException();
        }
    }
}
