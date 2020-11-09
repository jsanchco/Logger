namespace Client.ConsumeAPIConsole.Logger
{
    #region Using

    using System.Threading.Tasks;

    #endregion

    public enum LogEventLevel
    {
        INF = 2,
        ERR = 4
    }

    public interface ILogger
    {
        Task ConfigureLogger();
        Task WriteLogger(string message, LogEventLevel logEventLevel = LogEventLevel.INF, bool printConsole = true, bool printIn = true);
    }
}
