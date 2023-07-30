using OctoBackend.Domain.EventBus;

namespace OctoBackend.Application.Abstractions
{
    public interface IEventBusSubscriptionManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        void Clear();

        string GetEventKey<T>();
        Type GetEventTypeByName(string eventName);
        bool HasSubscriptionForEvent(string eventName);
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        void AddSubscription<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;

        void RemoveSubscription<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
       
    }
}
