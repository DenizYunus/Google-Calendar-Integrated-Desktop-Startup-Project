using OctoBackend.Domain.Models;

namespace OctoBackend.Application.Features.Queries.Todo.GetByCategory
{
    public class GetTodoByCategoryResponse
    {
        public ICollection<TaskTodo> Tasks { get; set; } = new HashSet<TaskTodo>();
        public DateTime DeadLine { get; set; }
    }
}
