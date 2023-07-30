using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Event.CollaboratorApproval
{
    public class CollaboratorApprovalCommand : IRequest<Response>
    {
        public string OwnerID { get; set; } = null!;
        public string ApprovalToken { get; set; } = null!;
        public string EventID { get; set; } = null!;
    }
}
