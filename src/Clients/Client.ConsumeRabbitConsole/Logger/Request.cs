using System;

namespace Client.ConsumeRabbitConsole.Logger
{
    public class Request
    {
        public int Lebel { get; set; }
        public string Information { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
