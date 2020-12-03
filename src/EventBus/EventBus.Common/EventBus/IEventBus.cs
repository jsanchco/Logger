using EventBus.Common.Commands;
using EventBus.Common.Events;
using System.Threading.Tasks;

namespace EventBus.Common.EventBus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;
        void Publish<T>(T @event) where T : Event;
        void Subscribe<T, TH>() where T  : Event
                                where TH : IEventHandler;
    }
}
