using EventBus.Common.Events;
using System;

namespace EventBus.Common.ModelsEvents
{
    public class LoggerEventQueue : Event
    {
        public DateTime CreateDateLogger { get; set; }
        public Lebel Lebel { get; set; }
        public string Information { get; set; }
    }

    public enum Lebel
    {
        Debug = 1,
        Information,
        Error
    }
}
