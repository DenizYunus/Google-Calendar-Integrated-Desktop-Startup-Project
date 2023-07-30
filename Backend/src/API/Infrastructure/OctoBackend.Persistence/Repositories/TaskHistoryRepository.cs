using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Domain.Collections.History;
using OctoBackend.Persistence.Repositories.BaseRepository;
using OctoBackend.Persitence.Contexts;

namespace OctoBackend.Persistence.Repositories
{
    public class TaskHistoryRepository : MongoRepository<TaskHistoryCollection>, ITaskHistoryRepository
    {
        public TaskHistoryRepository(OctoDBContext context) : base(context)
        {
        }
    }
}
