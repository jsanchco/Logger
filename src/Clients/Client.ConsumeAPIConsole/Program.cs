﻿using Client.ConsumeAPIConsole.Logger;
using Client.ConsumeAPIConsole.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

            var cont = 1;
            var random = new Random();
            do
            {
                while (!Console.KeyAvailable)
                {
                    var url = "http://localhost:7000/api/v1/Queue";

                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var requestSerialize = JsonConvert.SerializeObject(
                        new Request
                        {
                            Information = $"Logger Nº {cont}",
                            Lebel = random.Next(1, 4)
                        });

                    var httpContent = new StringContent(requestSerialize, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, httpContent);
                    if (!response.IsSuccessStatusCode)
                        await logger.WriteLogger($"Error in PostAsync [{cont}]", LogEventLevel.ERR);

                    cont++;
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
