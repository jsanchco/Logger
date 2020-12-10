using EventBus.AzureServiceBus.Configuration;
using EventBus.Common.Commands;
using EventBus.Common.EventBus;
using EventBus.Common.Events;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;

        public AzureEventBus(
            IMediator mediator,
            IServiceScopeFactory serviceScopeFactory,
            AzureServiceBusConnectionConfiguration azureServiceBusConnectionConfiguration)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _azureServiceBusConnectionConfiguration = azureServiceBusConnectionConfiguration;

            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
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
            var eventName = typeof(T).Name;
            var handlerEventType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(x => x.GetType() == handlerEventType))
            {
                throw new ArgumentException($"Handler {handlerEventType} is registred yet by {eventName}");
            }

            _handlers[eventName].Add(handlerEventType);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            var queueClient = new QueueClient(
                _azureServiceBusConnectionConfiguration.AzureServiceBusConnectionString,
                _azureServiceBusConnectionConfiguration.NameQueue);

            queueClient.RegisterMessageHandler(async (Microsoft.Azure.ServiceBus.Message message, CancellationToken token) => {
                var payload = JsonSerializer.Deserialize<T>(
                    Encoding.UTF8.GetString(message.Body)
                );

                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null)
                            continue;

                        var eventType = _eventTypes.SingleOrDefault(x => x.Name == eventName);

                        var concrectType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concrectType.GetMethod("Handler").Invoke(handler, new object[] { payload });
                    }
                }
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
