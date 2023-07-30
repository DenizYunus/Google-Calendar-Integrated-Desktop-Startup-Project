using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Todo.Add
{
    public class AddTaskCommand : IRequest<CreateResponse<AddTaskResponse>>
    {
        public string Task { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
