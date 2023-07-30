
using MediatR;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Event.CollaboratorApproval
{
    public class CollaboratorApprovalHandler : IRequestHandler<CollaboratorApprovalCommand, Response>
    {
        private readonly IEventService _eventService;

        public CollaboratorApprovalHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Response> Handle(CollaboratorApprovalCommand command, CancellationToken cancellationToken)
        {
            return await _eventService.ApproveEventAsCollaboratorAsync(command);
        }
    }
}
