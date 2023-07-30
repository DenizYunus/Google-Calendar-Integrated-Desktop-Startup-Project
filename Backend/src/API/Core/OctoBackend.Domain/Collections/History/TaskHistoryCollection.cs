using OctoBackend.Domain.Collections.BaseCollection;
using OctoBackend.Domain.Models;

namespace OctoBackend.Domain.Collections.History
{
    public class TaskHistoryCollection : BaseMongoCollection
    {
        public string UserName { get; set; } = null!;
        public ICollection<TaskTodoHistory> DeletedTasks { get; set; } = null!;       
    }
}
