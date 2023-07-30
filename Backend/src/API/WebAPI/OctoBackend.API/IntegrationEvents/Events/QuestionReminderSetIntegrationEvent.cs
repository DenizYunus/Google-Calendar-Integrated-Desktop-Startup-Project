using OctoBackend.Domain.EventBus;

namespace OctoBackend.API.IntegrationEvents.Events
{
    public class QuestionReminderSetIntegrationEvent : IntegrationEvent
    {
        public DateTime ReminderDate { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
