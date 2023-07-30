using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OctoBackend.Domain.Collections.BaseCollection;
using OctoBackend.Domain.Models;

namespace OctoBackend.Domain.Collections
{
    public class EventCollection : BaseMongoCollection
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? MeetingLink { get; set; }
        public DateTime StartAt { get; set; } 
        public DateTime EndAt { get; set; }
        public ICollection<EventCollaborator> Collaborators { get; set; } = new HashSet<EventCollaborator>();
    }
}
