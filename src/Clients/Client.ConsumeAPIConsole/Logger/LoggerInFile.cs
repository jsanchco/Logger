namespace Client.ConsumeAPIConsole.Logger
{
    #region Using

    using Client.ConsumeAPIConsole.Models;
    using Serilog;
    using System;
    using System.Threading.Tasks;

    #endregion

    public class LoggerInFile : Logger, ILogger
    {
        public LoggerInFile(AppConfig appConfig) : base(appConfig)
        {

        }

        public async Task ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .WriteTo.File($"logs\\{_appConfig.NameLog}.txt", outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
             .CreateLogger();

            await WriteLogger("Start task ...", LogEventLevel.INF, false);
        }

        public async Task WriteLogger(string message, LogEventLevel logEventLevel = LogEventLevel.INF, bool printConsole = true, bool printIn = true)
        {
            if (printConsole)
            {
                var consoleLine = $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss} {Enum.GetName(typeof(LogEventLevel), logEventLevel)}] {message}";
                await PrintInConsole(consoleLine);
            }

            if (printIn)
            {
                switch (logEventLevel)
                {
                    case LogEventLevel.INF:
                        Log.Write(Serilog.Events.LogEventLevel.Information, message);
                        break;

                    case LogEventLevel.ERR:
                        Log.Write(Serilog.Events.LogEventLevel.Error, message);
                        break;

                    default:
                        Log.Write(Serilog.Events.LogEventLevel.Information, message);
                        break;
                }                
            }
        }
    }
}
