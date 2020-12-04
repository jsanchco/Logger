using EventBus.Common.Commands;
using EventBus.Common.Configuration;
using EventBus.Common.EventBus;
using EventBus.Common.Events;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using System.Threading;
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
            var queueClient = new QueueClient(
                _azureServiceBusConnectionConfiguration.AzureServiceBusConnectionString,
                _azureServiceBusConnectionConfiguration.NameQueue);

            var json = JsonSerializer.Serialize(@event);

            queueClient.SendAsync(
                new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(json))
            );

            queueClient.CloseAsync();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler
        {
            var queueClient = new QueueClient(
                _azureServiceBusConnectionConfiguration.AzureServiceBusConnectionString,
                _azureServiceBusConnectionConfiguration.NameQueue);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(async (Microsoft.Azure.ServiceBus.Message message, CancellationToken token) => {
                var payload = JsonSerializer.Deserialize<T>(
                    Encoding.UTF8.GetString(message.Body)
                );

                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                //await handler.Execute(payload);
            }, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            System.Diagnostics.Debug.WriteLine(exceptionReceivedEventArgs.Exception.Message);

            // your custom message log
            return Task.CompletedTask;
        }
    }
}
