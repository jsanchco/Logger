using Client.ConsumeRabbitConsole.Logger;
using Client.ConsumeRabbitConsole.Models;
using Common.EventBus;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client.ConsumeRabbitConsole
{
    class Program
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
                    services.AddDbContext<MyDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("MyDatabase")));

                    services.AddScoped<IMyDbContext>(provider => provider.GetService<MyDbContext>());
                    services.AddMediatR(Assembly.GetExecutingAssembly());
                    services.AddSingleton<IHostedService, ConsoleApp>();
                });



        static async Task Main()
        {
            var cfg = InitOptions<AppConfig>();

            var logger = new LoggerInFile(cfg);
            await logger.ConfigureLogger();
            logger.Init();

            var cont = 1;
            var random = new Random();
            var start = DateTime.Now;
            var sentItems = 0;

            Console.WriteLine("Press Esc key to stop ...");
            Console.WriteLine("");
            do
            {
                //while (!Console.KeyAvailable && cont < 10001)
                while (!Console.KeyAvailable)
                {
                    cont++;
                    sentItems++;

                    var now = DateTime.Now;
                    if ((now - start).Seconds == 10)
                    {
                        await logger.WriteLogger($"Sent Items: {sentItems}");
                        start = DateTime.Now;
                        sentItems = 0;
                    }
                }

                //Console.WriteLine("");
                //Console.WriteLine("Sent 10000 items");
                //Console.WriteLine("");               
                //Console.WriteLine("Press any key to exit ...");
                //Console.ReadKey();
                //Environment.Exit(0);

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("");
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }

        #region Add appSettings.json
        private static T InitOptions<T>() where T : new()
        {
            var config = InitConfig();
            return config.Get<T>();
        }

        private static IConfigurationRoot InitConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
        #endregion
    }
}