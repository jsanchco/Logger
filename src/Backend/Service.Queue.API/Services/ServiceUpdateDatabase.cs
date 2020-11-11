using Common.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.Service.Command;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Queue.API.Services
{
    public class ServiceUpdateDatabase : IHostedService, IDisposable
    {
        private readonly IServiceList<Logger> _serviceList;
        private readonly ILogger<ServiceUpdateDatabase> _logger;
        private readonly IMediator _mediator;

        private int executionCount = 0;    
        private Timer _timer;

        public ServiceUpdateDatabase(
            IServiceList<Logger> serviceList, 
            ILogger<ServiceUpdateDatabase> logger,
            IMediator mediator)
        {
            _serviceList = serviceList;
            _logger = logger;
            _mediator = mediator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(
                DoWork, 
                null, 
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                $"Timed Hosted Service is working. Count: {count}, Items: {_serviceList.Count()}");

            if (_serviceList.Count() > 0)
            {
                _mediator.Send(new CreateManyLoggerCommand
                {
                    Loggers = _serviceList.GetItems()
                });
                _serviceList.RemoveAllItems();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
