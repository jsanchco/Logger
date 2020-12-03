using Common.EventBus.Commands;
using Common.EventBus.Events;
using System.Threading.Tasks;

namespace Common.EventBus.EventBus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;
        void Publish<T>(T @event) where T : Event;
        void Subscribe<T, TH>() where T  : Event
                                where TH : IEventHandler;
    }
}
