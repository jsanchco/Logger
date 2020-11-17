using Common.EventBus.Events;
using System.Threading.Tasks;

namespace Common.EventBus.BusRabbit
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event  
    {
        Task Handler(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
