﻿namespace EventBus.Rabbit.Configuration
{
    public class RabbitConnectionConfiguration
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string RabbitConnectionString { get; set; }
    }
}
