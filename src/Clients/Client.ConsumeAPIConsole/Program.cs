using Client.ConsumeAPIConsole.Logger;
using Client.ConsumeAPIConsole.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Client.ConsumeAPIConsole
{
    class Program
    {
        static async Task Main()
        {
            var cfg = InitOptions<AppConfig>();

            var logger = new LoggerInFile(cfg);
            await logger.ConfigureLogger();
            logger.Init();
            
            do
            {
                while (!Console.KeyAvailable)
                {
                    await logger.WriteLogger("Hi!!!");
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
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
