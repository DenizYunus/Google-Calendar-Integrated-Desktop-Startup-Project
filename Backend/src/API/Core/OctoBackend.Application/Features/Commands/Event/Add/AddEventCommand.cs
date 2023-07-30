using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Event.Add
{
    public class AddEventCommand : IRequest<Response>
    {
        public string Name { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public ICollection<string> Collaborators { get; set; } = new HashSet<string>();
    }
}
