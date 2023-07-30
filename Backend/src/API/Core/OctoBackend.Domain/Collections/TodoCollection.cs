using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OctoBackend.Domain.Collections.BaseCollection;
using OctoBackend.Domain.Enums;
using System.Text.Json.Serialization;
using OctoBackend.Domain.Models;

namespace OctoBackend.Domain.Collections
{
    public class TodoCollection : BaseMongoCollection
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; } = null!;
        public ICollection<TaskTodo> Tasks { get; set; } = new HashSet<TaskTodo>();
        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoCategory Category { get; set; } 
      
        public DateTime DeadLine { get; set; }
    }
}
