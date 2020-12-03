using Common.EventBus.Events;
using System.Threading.Tasks;

namespace Common.EventBus.EventBus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event  
    {
        Task Handler(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
