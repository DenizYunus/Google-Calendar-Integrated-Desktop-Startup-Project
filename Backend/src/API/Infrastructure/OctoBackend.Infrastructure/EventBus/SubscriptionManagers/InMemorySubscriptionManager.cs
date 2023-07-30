using OctoBackend.Application.Abstractions;
using OctoBackend.Domain.EventBus;

namespace OctoBackend.Infrastructure.EventBus.SubscriptionManagers
{
    public class InMemorySubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string>? OnEventRemoved;
        public Func<string, string> _eventNameGetter;

        public InMemorySubscriptionManager(Func<string, string> eventNameGetter)
        {
            _handlers = new();
            _eventTypes = new();
            _eventNameGetter = eventNameGetter;

        }
        public bool IsEmpty => !_handlers.Keys.Any();
        public void Clear() => _handlers.Clear();
        public string GetEventKey<T>()
            => _eventNameGetter(typeof(T).Name);

        public Type GetEventTypeByName(string eventName)
            => _eventTypes.SingleOrDefault(t => t.Name == eventName)!;

        public bool HasSubscriptionForEvent(string eventName)
            => _handlers.ContainsKey(eventName);

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
          => _handlers[eventName];

        public void AddSubscription<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();

            AddSubscription(typeof(TEventHandler), eventName);

            if(!_eventTypes.Contains(typeof(TEvent)))
                _eventTypes.Add(typeof(TEvent));
        }

        private void AddSubscription(Type type, string eventName)
        {
            if(!HasSubscriptionForEvent(eventName))
                _handlers.Add(eventName, new List<SubscriptionInfo>());

            if (_handlers[eventName].Any(s => s.HandlerType == type))
                throw new ArgumentException($"Handler Type {type.Name} already registered for '{eventName}'", nameof(type));

            _handlers[eventName].Add(SubscriptionInfo.Typed(type));
        }
        public void RemoveSubscription<TEvent, TEventHandler>()
                    where TEvent : IntegrationEvent
                    where TEventHandler : IIntegrationEventHandler<TEvent>
        {
           var handlerToRemove = FindSubscriptionToRemove<TEvent, TEventHandler>();
           var eventName = GetEventKey<TEvent>();
            RemoveHandler(eventName, handlerToRemove!);
        }

        private void RemoveHandler(string eventName, SubscriptionInfo subToRemove)
        {
            if(subToRemove != null)
            {
                _handlers[eventName].Remove(subToRemove);

                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);

                    if(eventType != null)
                        _eventTypes.Remove(eventType);

                    RaiseOnEventRemoved(eventName);
                }
            }
        }
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        private SubscriptionInfo? FindSubscriptionToRemove<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            return FindSubscriptionToRemove(eventName, typeof(TEventHandler));
        }

        private SubscriptionInfo? FindSubscriptionToRemove(string eventName, Type type)
        {
            if (!HasSubscriptionForEvent(eventName))
                return null;

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == type);
        }
    }
}
