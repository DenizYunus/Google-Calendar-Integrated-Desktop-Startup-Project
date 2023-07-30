
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OctoBackend.Domain.Enums;
using System.Text.Json.Serialization;

namespace OctoBackend.Domain.Models
{
    public class TaskTodoHistory
    {
        public string TaskName { get; set; } = null!;
        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoCategory TaskCategory { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
