using MongoDB.Bson;
using MongoDB.Driver;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Domain.Collections;
using OctoBackend.Persistence.Repositories.BaseRepository;
using OctoBackend.Persitence.Contexts;
using System.Text.RegularExpressions;

namespace OctoBackend.Persistence.Repositories
{
    public class UserRepository : MongoRepository<UserCollection>, IUserRepository
    {
        public UserRepository(OctoDBContext context) : base(context)
        {
 
        }

        public async Task<ICollection<UserCollection>> GetByUserNameFilterAsync(string userNameFilter)
        {
            var filterBuilder = Builders<UserCollection>.Filter;
            var filter = filterBuilder.Regex(x => x.UserName, new BsonRegularExpression($"^{Regex.Escape(userNameFilter)}"));

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
