using Common.Domain.Entities;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Queue.API.Services
{
    public class ServiceUpdateDatabase : BackgroundService
    {
        private readonly IServiceList<Logger> _serviceList;

        public ServiceUpdateDatabase(IServiceList<Logger> serviceList)
        {
            _serviceList = serviceList;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}
