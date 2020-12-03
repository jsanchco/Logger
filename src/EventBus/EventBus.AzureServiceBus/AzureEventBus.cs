using EventBus.Common.Commands;
using EventBus.Common.Configuration;
using EventBus.Common.EventBus;
using EventBus.Common.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace EventBus.AzureServiceBus
{
    public class AzureEventBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AzureServiceBusConnectionConfiguration _azureServiceBusConnectionConfiguration;

        public AzureEventBus(
            IMediator mediator,
            IServiceScopeFactory serviceScopeFactory,
            AzureServiceBusConnectionConfiguration azureServiceBusConnectionConfiguration)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _azureServiceBusConnectionConfiguration = azureServiceBusConnectionConfiguration;
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
