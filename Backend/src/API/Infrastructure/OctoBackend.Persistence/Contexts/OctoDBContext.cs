                                                                                                                                                                                                                                                                                                                    
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Collections.BaseCollection;
using OctoBackend.Domain.Collections.History;

namespace OctoBackend.Persitence.Contexts
{
    public class OctoDBContext
    {
        private readonly IConfiguration _config;
        private readonly IMongoDatabase _database;
        private readonly Dictionary<Type, string> _collections;

        public OctoDBContext(IConfiguration config)
        {
            _config = config;
            var client = new MongoClient(config["MongoDB:ConnectionURI"]);
            _database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _collections = GetCollections();

        }
        public IMongoCollection<BaseMongoCollection> GetCollection<BaseMongoCollection>()
        {
            return _database.GetCollection<BaseMongoCollection>(_collections[typeof(BaseMongoCollection)]);
        }

        public Dictionary<Type, string> GetCollections()
        {
            Dictionary<Type, string> collections = new()
            {
                { typeof(UserCollection), _config["MongoDB:UserCollectionName"]! },
                { typeof(AvatarCollection), _config["MongoDB:AvatarCollectionName"]! },
                { typeof(EventCollection), _config["MongoDB:EventCollectionName"]! },
                { typeof(TodoCollection), _config["MongoDB:TodoCollectionName"]!},
                { typeof(TaskHistoryCollection), _config["MongoDB:TaskHistoryCollectionName"]!}
            };
            return collections;
        }
    }
}

