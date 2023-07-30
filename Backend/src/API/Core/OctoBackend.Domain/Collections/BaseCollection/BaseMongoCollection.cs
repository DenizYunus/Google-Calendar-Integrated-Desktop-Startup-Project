using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;

namespace OctoBackend.Domain.Collections.BaseCollection
{
    public class BaseMongoCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    }
}
