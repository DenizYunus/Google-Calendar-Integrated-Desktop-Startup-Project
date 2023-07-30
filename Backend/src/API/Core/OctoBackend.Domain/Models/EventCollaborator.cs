using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OctoBackend.Domain.Enums;
using System.Text.Json.Serialization;

namespace OctoBackend.Domain.Models
{
    public class EventCollaborator
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; } = null!;
        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ApprovalStatus ApprovalStatus { get; set; }
        public string? ApprovalToken { get; set; } = null!;
        public DateTime? ApprovalTokenExpireDate { get;  set; }

        public EventCollaborator(string id, string token)
        {
            ID = id;
            ApprovalToken = token;
            ApprovalStatus = ApprovalStatus.Awaiting;
            ApprovalTokenExpireDate = DateTime.UtcNow.AddMinutes(10);
        }
    }
}
