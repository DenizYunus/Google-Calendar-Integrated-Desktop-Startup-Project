using OctoBackend.Application.Abstractions;
using OctoBackend.Domain.EventBus;

namespace OctoBackend.API.Extensions
{
    public class ConfigureSubscription
    {
        public static void AddSubscription<TEvent, TEventHandler>(IServiceCollection serviceCollection)
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var eventBus = serviceProvider.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TEvent, TEventHandler>();
        }
    }
}
