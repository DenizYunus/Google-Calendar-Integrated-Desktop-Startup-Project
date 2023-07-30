
using OctoBackend.Application.Features.Commands.Event.Add;
using OctoBackend.Application.Features.Commands.Event.CollaboratorApproval;
using OctoBackend.Application.Features.Commands.Event.CreateInstanceEvent;
using OctoBackend.Application.Features.Queries.Event.GetUpcomings;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;
using System.Security.Claims;

namespace OctoBackend.Application.Abstractions.Services
{
    public interface IEventService
    {
        ///TODO: THE RETURN TYPE OF FUNCTIONS SHOULDN'T SAME BE WITH THE COMMAND RESPONSES
        Task<Response> AddAsync(AddEventCommand command, IEnumerable<Claim> userClaims);
        Task<GetOneResponse<CreateInstanceEventResponse>> CreateInstanceEventAsync(CreateInstanceEventCommand command);
        Task<Response> ApproveEventAsCollaboratorAsync(CollaboratorApprovalCommand command);
        Task<GetManyResponse<GetUpomingEventsResponse>> GetUpcomingsAsync(GetUpcomingEventsQuery command, IEnumerable<Claim> userClaims);
    }
}
