using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Todo.Update
{
    public class SetTaskStatusCommand : IRequest<Response>
    {
        public string Category { get; set; } = null!;
        public string TaskID { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
