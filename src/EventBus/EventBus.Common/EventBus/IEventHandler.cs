using EventBus.Common.Events;
using System.Threading.Tasks;

namespace EventBus.Common.EventBus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event  
    {
        Task Handler(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
