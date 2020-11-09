namespace Client.ConsumeAPIConsole.Logger
{
    #region Using

    using Client.ConsumeAPIConsole.Models;
    using System;
    using System.Threading.Tasks;

    #endregion

    public abstract class Logger
    {
        protected AppConfig _appConfig;

        public Logger(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public void Init()
        {
            PrintTitle();
        }

        protected async Task PrintInConsole(string line)
        {
            await Console.Out.WriteLineAsync(line);
        }

        private void PrintTitle()
        {
            var lengthTotal = _appConfig.Title.Length + 20;
            Console.WriteLine($"{new string('*', lengthTotal)}");
            Console.WriteLine($"*         {_appConfig.Title}         *");
            Console.WriteLine($"{new string('*', lengthTotal)}");

            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
