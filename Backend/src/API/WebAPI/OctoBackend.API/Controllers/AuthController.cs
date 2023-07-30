using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctoBackend.Application.Features.Commands.User.GoogleLogin;
using OctoBackend.Application.Features.Commands.User.GoogleSignUpComplete;
using OctoBackend.Application.Features.Commands.User.GoogleSignUpStart;
using OctoBackend.Application.Models;

namespace OctoBackend.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("google-signup-start")]
        public async Task<IActionResult> GoogleSignUpStart(GoogleSignUpStartCommand command)
        {
            var result = await _mediator.Send(command);

            if(result.Success)
                return Ok(result.Result);

            Message message = result.Message!;

            if (message.Code == MessageCode.Conflict)
                return Conflict(message.Content);

            return BadRequest(message.Content);
        }

        [Authorize("Uncompletedregistration")]
        [HttpPost("google-signup-complete")]
        public async Task<IActionResult> GoogleSignUpComplete(GoogleSignUpCompleteCommand command)
        {
            var result = await _mediator.Send(command);

            if(result.Success)
                return Ok();

            Message message = result.Message!;

            if(message.Code == MessageCode.NotFound)
                return NotFound(message.Content);

            if (message.Code == MessageCode.Conflict)
                return Conflict(message.Content);

            return BadRequest(message.Content);
        }
        ///TODO: IMPLEMENT A FORBID ROLE
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok(result.Result);

            Message message = result.Message!;

            if (message.Code == MessageCode.NotFound)
                return NotFound(message.Content);

            if (message.Code == MessageCode.Forbidden)
                return Forbid();

            return BadRequest(message.Content);
        }


    }
}
