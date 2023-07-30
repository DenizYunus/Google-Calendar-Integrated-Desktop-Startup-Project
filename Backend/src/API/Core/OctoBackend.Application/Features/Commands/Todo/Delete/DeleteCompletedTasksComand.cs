using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Todo.Delete
{
    public class DeleteCompletedTasksComand : IRequest<Response>
    {
        public string Category { get; set; } = null!;
    }
}
