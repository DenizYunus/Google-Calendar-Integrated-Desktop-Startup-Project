
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Enums;
using OctoBackend.Domain.Models;

namespace OctoBackend.Application.Abstractions.Repositories
{
    public interface ITodoRepository : IMongoRepository<TodoCollection>
    {
        Task<TodoCollection> GetByCategoryAsync(string category, string userID);
        Task InitializeCategoriesAsync(string userID);
    }
}
