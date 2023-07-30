using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctoBackend.Application.Features.Commands.User.CompleteCrispyQuestions;
using OctoBackend.Application.Features.Commands.User.RemindMeQuestions;
using OctoBackend.Application.Features.Commands.User.UpdateUser;
using OctoBackend.Application.Features.Commands.User.UploadCustomPhoto;
using OctoBackend.Application.Features.Queries.User.GetByUserName;
using OctoBackend.Application.Features.Queries.User.GetByUserNameFilter;
using OctoBackend.Application.Models;

namespace OctoBackend.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize("User")]
        [HttpGet("getbyusernamefilter")]
        public async Task<IActionResult> GetByUserNameFilter([FromQuery] GetUserByUserNameFilterQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.Success)
                return Ok(result.Result);

            return BadRequest(result.Message);
        }

        [Authorize("User")]
        [HttpGet("getbyusername")]
        public async Task<IActionResult> GetByUserName([FromQuery] GetUserByUserNameQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.Success)
                return Ok(result.Result);

            Message message = result.Message!;

            if (message.Code == MessageCode.NotFound)
                return NotFound(message.Content);

            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpPost("upload-photo")]
        public async Task<IActionResult> Upload([FromForm] UploadCustomPhotoCommand command) 
        {
            command.File = Request.Form.Files[0];
            var result = await _mediator.Send(command);

            return result.Success == true ? Ok(result.Result) : BadRequest(result.Message!.Content); 
        }

        [Authorize("Uncompletedregistration")]
        [HttpPost("complete-questions")]
        public async Task<IActionResult> CompleteQuestions([FromBody] CompleteCrispyQuestionsCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success == true ? Ok() : BadRequest(result.Message!.Content);
        }

        [Authorize]
        [HttpPost("remind-questions")]
        public async Task<IActionResult> RemindQuestions([FromBody] RemindQuestionsCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success == true ? Ok() : BadRequest(result.Message!.Content);
        }


        [Authorize("User")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok();

            Message message = result.Message!;

            if (message.Code == MessageCode.NotFound)
                return NotFound(message.Content);

            if (message.Code == MessageCode.Conflict)
                return Conflict(message.Content);

            return BadRequest(message.Content);
        }
    }
}
