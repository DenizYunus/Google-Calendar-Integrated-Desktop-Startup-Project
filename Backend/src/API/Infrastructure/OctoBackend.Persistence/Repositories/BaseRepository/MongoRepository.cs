using MongoDB.Driver;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Domain.Collections.BaseCollection;
using OctoBackend.Persitence.Contexts;
using System.Linq.Expressions;

namespace OctoBackend.Persistence.Repositories.BaseRepository
{
    public class MongoRepository<TCollection> : IMongoRepository<TCollection> where TCollection : BaseMongoCollection
    {
        private readonly OctoDBContext _context;
        protected readonly IMongoCollection<TCollection> _collection;

        public MongoRepository(OctoDBContext context)
        {
            _context = context;
            _collection = _context.GetCollection<TCollection>();
        }

        public async Task<ICollection<TCollection>> GetAllAsync()  
            => await _collection.AsQueryable().ToListAsync();      

        public async Task<TCollection> GetSingleAsync(Expression<Func<TCollection, bool>> predicate)
            => await _collection.Find(predicate).SingleOrDefaultAsync();

        public async Task<TCollection> GetByIdAsync(string id)
        { 
                var filter = Builders<TCollection>.Filter.Eq("Id", id);
                return await _collection.Find(filter).FirstOrDefaultAsync();        
        }

        public async Task InsertOneAsync(TCollection collection)
            => await _collection.InsertOneAsync(collection);

        public async Task InsertManyAsync(ICollection<TCollection> collections)
            =>   await _collection.InsertManyAsync(collections);

        public async Task ReplaceOneAsync(TCollection collection, string id)
        {
                var filter = Builders<TCollection>.Filter.Eq("Id", id);
                await _collection.ReplaceOneAsync(filter, collection);
        }

        public async Task DeleteOneAsync(string id)
        {
            var filter = Builders<TCollection>.Filter.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }

    }
}

