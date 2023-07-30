using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OctoBackend.Domain.Collections.BaseCollection;
using OctoBackend.Domain.Enums;
using OctoBackend.Domain.Models;
using System.Text.Json.Serialization;

namespace OctoBackend.Domain.Collections
{
    public class UserCollection : BaseMongoCollection
    {
        public string EmailAddress { get; set; } = null!;
        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoleType Role { get; set; } = RoleType.Uncompletedregistration;
        public string? UserName { get; set; }
        public string? Name { get; set;}
        public string? ProfilePictureURL { get; set; }
        public string? Industry { get; set; }
        public string? EntrepreneurField { get; set; }
        public int WorkingHoursInADay { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<string> RequestedServices { get; set; } = new HashSet<string>();
        public ICollection<string> RequestedCollaborations { get; set; } = new HashSet<string>();
        public RefreshToken? RefreshToken { get; set; }
    }
}
