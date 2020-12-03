using Common.Domain.Entities;
using EventBus.Common.EventBus;
using EventBus.Common.ModelsEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Service.Command;
using System;
using System.Threading.Tasks;

namespace Service.Queue.API.BrokerHandler
{
    public class EventLoggerHandler : IEventHandler<LoggerEventQueue>
    {
        private readonly ILogger<EventLoggerHandler> _logger;
        private readonly IMediator _mediator;

        public EventLoggerHandler(
            ILogger<EventLoggerHandler> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public Task Handler(LoggerEventQueue @event)
        {
            _logger.LogInformation($"In RabbitEventLoggerHandler [Logger] -> {@event.Information}");

            _mediator.Send(new CreateLoggerCommand
            {
                Logger = new Logger
                {
                    Information = @event.Information,
                    Lebel = (Common.Domain.Entities.Lebel)Enum.ToObject(typeof(Common.Domain.Entities.Lebel), @event.Lebel),
                    Timestamp = @event.Timestamp
                }
            });

            return Task.CompletedTask;
        }
    }
}
