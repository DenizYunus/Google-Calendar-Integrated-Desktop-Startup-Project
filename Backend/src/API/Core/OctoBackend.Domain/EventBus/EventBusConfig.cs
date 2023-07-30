

namespace OctoBackend.Domain.EventBus
{
    public class EventBusConfig
    {
        public int ConnectionRetryCount { get; set; } = 5;
        public string DefaultTopicName { get; set; } = "EventBus";
        public string ConnectionString { get; set; } = string.Empty;
        public string SubscriberClientAppName { get; set; } = string.Empty;
        public string EventNamePrefix { get; set; } = string.Empty;
        public string EventNameSuffix { get; set; } = string.Empty;
        public object? Connection { get; set; }
        public bool DeleteEventPrefix => !String.IsNullOrEmpty(EventNamePrefix);
        public bool DeleteEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);

    }
}
