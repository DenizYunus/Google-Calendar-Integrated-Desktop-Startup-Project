using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OctoBackend.Domain.EventBus
{
    public class IntegrationEvent
    {
        [JsonProperty]
        public Guid ID { get; set; }
        [JsonProperty]
        public DateTime CreatedDate { get; private set; }
        public IntegrationEvent()
        {
            ID = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }
}