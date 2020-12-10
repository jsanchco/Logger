using EventBus.Rabbit.Configuration;

namespace Client.ConsumeRabbitConsole.Models
{
    public class AppConfig
    {
        public string Title { get; set; }
        public Papertrail Papertrail { get; set; } = new Papertrail();
        public RabbitConnectionConfiguration Rabbit { get; set; } = new RabbitConnectionConfiguration();
    }

    public class Papertrail
    {
        public string host { get; set; }
        public int port { get; set; }
    }
}
