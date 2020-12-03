using Common.EventBus.Events;
using System;

namespace Common.EventBus.ModelsEvents
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
