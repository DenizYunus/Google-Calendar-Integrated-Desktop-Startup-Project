using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctoBackend.API.Attributes;
using OctoBackend.Application.Features.Commands.Event.Add;
using OctoBackend.Application.Features.Commands.Event.CollaboratorApproval;
using OctoBackend.Application.Features.Commands.Event.CreateInstanceEvent;
using OctoBackend.Application.Features.Queries.Event.GetUpcomings;
using OctoBackend.Application.FromBodyModels.Event;
using OctoBackend.Application.Models;

namespace OctoBackend.API.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize("User")]
        [HttpGet("get-upcomings")]
        public async Task<IActionResult> GetUpcomings([FromQuery] GetUpcomingEventsQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.Success)
                return Ok(result.Result);

            return BadRequest(result.Message!.Content);
        }

        [HttpGet("collaborator-approval")]
        public async Task<IActionResult> CollaboratorApproval([ValidateObjectID][FromQuery] string ownerID, [FromQuery] string approvalToken, [ValidateObjectID][FromQuery] string eventID)
        {
            CollaboratorApprovalCommand approvalCommand = new()
            {
                OwnerID = ownerID,
                ApprovalToken = approvalToken,
                EventID = eventID
            };

            var result = await _mediator.Send(approvalCommand);

            if (result.Success)
                return Ok();

            Message message = result.Message!;

            if (message.Code == MessageCode.NotFound)
                return NotFound(message.Content);

            return BadRequest(message.Content);
        }

        [Authorize("User")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]AddEventBody eventBody, [FromQuery][ValidateObjectIDCollection] ICollection<string> Collaborators)
        {
            var command = _mapper.Map<AddEventCommand>(eventBody);
            command.Collaborators = Collaborators;

            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok();

            Message message = result.Message!;

            if (message.Code == MessageCode.NotFound)
                return NotFound(message.Content);

            return BadRequest(message.Content);
        }

        [Authorize("User")]
        [HttpPost("create-instance")]
        public async Task<IActionResult> CreateInstance()
        {
            CreateInstanceEventCommand command = new();

            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok(result.Result);

            return BadRequest(result.Message!.Content);
        }
            
    }
}
