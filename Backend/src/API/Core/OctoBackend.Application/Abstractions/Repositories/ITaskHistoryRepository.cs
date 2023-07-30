using OctoBackend.Domain.Collections.History;

namespace OctoBackend.Application.Abstractions.Repositories
{
    public interface ITaskHistoryRepository : IMongoRepository<TaskHistoryCollection>
    {
    }
}
