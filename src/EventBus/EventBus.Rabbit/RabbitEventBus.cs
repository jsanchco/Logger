using EventBus.Common.Commands;
using EventBus.Common.Configuration;
using EventBus.Common.EventBus;
using EventBus.Common.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Rabbit
{
    public class RabbitEventBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitConnectionConfiguration _rabbitConnectionConfiguration;

        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;

        public RabbitEventBus(
            IMediator mediator,
            IServiceScopeFactory serviceScopeFactory,
            RabbitConnectionConfiguration rabbitConnectionConfiguration)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _rabbitConnectionConfiguration = rabbitConnectionConfiguration;

            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitConnectionConfiguration.HostName,
                UserName = _rabbitConnectionConfiguration.UserName,
                Password = _rabbitConnectionConfiguration.Password,
                Port = _rabbitConnectionConfiguration.Port
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
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

            var factory = new ConnectionFactory
            {
                HostName = _rabbitConnectionConfiguration.HostName,
                UserName = _rabbitConnectionConfiguration.UserName,
                Password = _rabbitConnectionConfiguration.Password,
                Port = _rabbitConnectionConfiguration.Port,

                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                if (_handlers.ContainsKey(eventName))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var subscriptions = _handlers[eventName];
                        foreach (var subscription in subscriptions)
                        {

                            var handler = scope.ServiceProvider.GetService(subscription);
                            if (handler == null)
                                continue;

                            var eventType = _eventTypes.SingleOrDefault(x => x.Name == eventName);
                            var eventDS = JsonConvert.DeserializeObject(message, eventType);

                            var concrectType = typeof(IEventHandler<>).MakeGenericType(eventType);
                            await (Task)concrectType.GetMethod("Handler").Invoke(handler, new object[] { eventDS });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error -> {ex.Message}");
            }
        }
    }
}
