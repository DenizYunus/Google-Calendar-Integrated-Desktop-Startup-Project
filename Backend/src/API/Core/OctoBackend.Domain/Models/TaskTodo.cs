using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OctoBackend.Domain.Enums;
using System.Text.Json.Serialization;

namespace OctoBackend.Domain.Models
{
    public class TaskTodo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Content { get; set; } = null!;
        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoStatus Status { get; set; } = TodoStatus.NextUp;
        public DateTime CreatedAt { get; set; }

        public TaskTodo(string content)
        {
            Content = content;
            Status = TodoStatus.NextUp;
            CreatedAt = DateTime.Now;
        }
    }
}
