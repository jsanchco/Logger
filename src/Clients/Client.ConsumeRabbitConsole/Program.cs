using Client.ConsumeRabbitConsole.Models;
using Client.ConsumeRabbitConsole.Services;
using Common.Logging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading.Tasks;

namespace Client.ConsumeRabbitConsole
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

                    // Add Papertrail to trace
                    var serviceProvider = services.BuildServiceProvider();
                    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                    var configuration = new Configuration<AppConfig>();
                    loggerFactory.AddSyslog(
                        configuration.Settings.Papertrail.host,
                        configuration.Settings.Papertrail.port);
                    services.AddSingleton(typeof(ILoggerFactory), loggerFactory);
                    // Add Papertrail to trace
                });
    }
}
