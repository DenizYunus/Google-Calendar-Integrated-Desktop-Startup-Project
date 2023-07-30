using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Todo.Update
{
    public class SetDeadlineCommand : IRequest<Response>
    {
        public string Category { get; set; } = null!;
        public DateTime Deadline { get; set; }
    }
}
