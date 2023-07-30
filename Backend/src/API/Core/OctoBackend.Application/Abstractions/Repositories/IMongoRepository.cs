using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections.BaseCollection;
using System.Linq.Expressions;

namespace OctoBackend.Application.Abstractions.Repositories
{
    public interface IMongoRepository<TCollection> where TCollection : BaseMongoCollection
    {
        Task<ICollection<TCollection>> GetAllAsync();
        Task<TCollection> GetSingleAsync(Expression<Func<TCollection, bool>> predicate);
        Task<TCollection> GetByIdAsync(string id);
        Task InsertOneAsync(TCollection collection);
        Task InsertManyAsync
            (ICollection<TCollection> collections);
        Task ReplaceOneAsync(TCollection collection, string id);
        Task DeleteOneAsync(string id);
    }
}
