using EventBus.Common.Configuration;

namespace Client.ConsumeAzureServiceBusConsole.Models
{
    public class AppConfig
    {
        public string Title { get; set; }
        public Papertrail Papertrail { get; set; } = new Papertrail();
        public AzureServiceBusConnectionConfiguration Azure { get; set; } = new AzureServiceBusConnectionConfiguration();
    }

    public class Papertrail
    {
        public string host { get; set; }
        public int port { get; set; }
    }
}
