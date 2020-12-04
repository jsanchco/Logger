using Client.ConsumeAzureServiceBusConsole.Models;
using Client.ConsumeAzureServiceBusConsole.Services;
using Common.Logging;
using EventBus.AzureServiceBus;
using EventBus.Common.EventBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading.Tasks;

namespace Client.ConsumeAzureServiceBusConsole
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            await hostBuilder.RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddSingleton<Configuration<AppConfig>>();
                    services.AddMediatR(Assembly.GetExecutingAssembly());
                    services.AddSingleton<IHostedService, ConsoleApp>();

                    var configuration = new Configuration<AppConfig>();
                    services.AddSingleton<IEventBus, AzureEventBus>(sp =>
                    {
                        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                        return new AzureEventBus(
                            sp.GetService<IMediator>(),
                            scopeFactory,
                            configuration.Settings.Azure);
                    });

                    // Add Papertrail to trace
                    var serviceProvider = services.BuildServiceProvider();
                    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                    loggerFactory.AddSyslog(
                        configuration.Settings.Papertrail.host,
                        configuration.Settings.Papertrail.port);
                    services.AddSingleton(typeof(ILoggerFactory), loggerFactory);
                    // Add Papertrail to trace
                });
    }
}
