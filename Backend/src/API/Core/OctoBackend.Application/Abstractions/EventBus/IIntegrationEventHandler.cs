using OctoBackend.Domain.EventBus;

namespace OctoBackend.Application.Abstractions
{
    public interface IIntegrationEventHandler<TIntegrationEvent>  where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent integrationEvent);
    }
}
