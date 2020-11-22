using Microsoft.Extensions.Configuration;
using System;

namespace Client.ConsumeRabbitConsole.Services
{
    public class Configuration<T> where T : new()
    {
        public T Settings;

        public Configuration()
        {
            Settings = InitOptions();
        }

        private static T InitOptions()
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
    }
}
