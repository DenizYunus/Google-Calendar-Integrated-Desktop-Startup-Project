using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using OctoBackend.Application.Abstractions;
using OctoBackend.Domain.EventBus;
using OctoBackend.Infrastructure.EventBus.SubscriptionManagers;

namespace OctoBackend.Infrastructure.EventBus
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider _serviceProvider;
        public readonly IEventBusSubscriptionManager _subManager;

        public EventBusConfig _config;

        public BaseEventBus(EventBusConfig config, IServiceProvider serviceProvider)
        {
            _config = config;
            _serviceProvider = serviceProvider;
            _subManager = new InMemorySubscriptionManager(ProcessEventName);
        }

        public virtual string GetSubName(string eventName)
        {
            return $"{_config.SubscriberClientAppName}{ProcessEventName(eventName)}";
        }

        public virtual string ProcessEventName(string eventName)
        {
            if(_config.DeleteEventPrefix)
                eventName= eventName.TrimStart(_config.EventNamePrefix.ToArray());

            if (_config.DeleteEventSuffix && eventName.EndsWith(_config.EventNameSuffix))           
                eventName = eventName.Replace(_config.EventNameSuffix, string.Empty);
            

            return eventName;
        }

        public virtual void Dispose()
        {
            _config = null!;
        }

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            eventName = ProcessEventName(eventName);

            var processed = false;

            if (_subManager.HasSubscriptionForEvent(eventName))
            {
                var subscriptions = _subManager.GetHandlersForEvent(eventName);

                using (var scope = _serviceProvider.CreateScope())
                {
                    foreach (var subscription in subscriptions) 
                    {
                        var handler = _serviceProvider.GetService(subscription.HandlerType);
                        if (handler == null) continue;

                        var eventType = _subManager.GetEventTypeByName($"{_config.EventNamePrefix}{eventName}{_config.EventNameSuffix}");
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType)!;

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, new object[] {integrationEvent})!;
                    }
                }
                processed = true;
            }
            return processed;
        }

        public abstract void Publish(IntegrationEvent integrationEvent);

        public abstract void Subscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>;
        public abstract void UnSubscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>;
    }
}
