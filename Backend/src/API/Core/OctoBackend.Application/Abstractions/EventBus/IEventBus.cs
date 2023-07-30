using OctoBackend.Domain.EventBus;

namespace OctoBackend.Application.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent integrationEvent);
        void Subscribe<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
        void UnSubscribe<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
    }
}
