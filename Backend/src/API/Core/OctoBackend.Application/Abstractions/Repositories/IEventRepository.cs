

using OctoBackend.Domain.Collections;

namespace OctoBackend.Application.Abstractions.Repositories
{
    public interface IEventRepository : IMongoRepository<EventCollection>
    {
        Task<ICollection<EventCollection>> GetNearestByCountAsync(int count, string userID);
    }
}
