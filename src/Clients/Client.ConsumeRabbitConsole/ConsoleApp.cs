using Client.ConsumeRabbitConsole.Models;
using Client.ConsumeRabbitConsole.Services;
using EventBus.Common.EventBus;
using EventBus.Common.ModelsEvents;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client.ConsumeRabbitConsole
{
    public class ConsoleApp : IHostedService
    {
        private readonly Configuration<AppConfig> _configuration;
        private readonly ILogger<ConsoleApp> _logger;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;

        public ConsoleApp(
            Configuration<AppConfig> configuration,
            ILogger<ConsoleApp> logger,
            IMediator mediator,
            IEventBus eventBus)
        {
            _configuration = configuration;
            _logger = logger;
            _mediator = mediator;
            _eventBus = eventBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {            
            PrintTitle();
            Process();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private Task PrintTitle()
        {
            var lengthTotal = _configuration.Settings.Title.Length + 20;
            Console.WriteLine($"{new string('*', lengthTotal)}");
            Console.WriteLine($"*         {_configuration.Settings.Title}         *");
            Console.WriteLine($"{new string('*', lengthTotal)}");

            Console.WriteLine("");
            Console.WriteLine("");

            return Task.CompletedTask;
        }

        private Task Process()
        {
            Console.WriteLine("Press [Enter] to send message or [Esc] to exit ...");
            Console.WriteLine("");
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    _logger.LogInformation("Send Item ...");

                    var now = DateTime.Now;
                    _eventBus.Publish(
                        new LoggerEventQueue
                        {
                            CreateDateLogger = now,
                            Lebel = Lebel.Information,
                            Information = $"[FROM RABBIT] Information in the moment {now:dd/MM/yyyy HH:mm:ss}"
                        });
                }

            } while (cki.Key != ConsoleKey.Escape);

            Console.WriteLine("");
            Console.WriteLine("[Ctrl+C] to Exit ...");

            return Task.CompletedTask;
        }
    }
}
