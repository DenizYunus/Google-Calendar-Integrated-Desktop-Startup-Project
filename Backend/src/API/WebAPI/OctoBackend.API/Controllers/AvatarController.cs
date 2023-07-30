using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Features.Commands.Avatar.Upload;
using OctoBackend.Application.Features.Queries.Avatar.GetAll;

namespace OctoBackend.API.Controllers
{
    [Route("api/avatar")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AvatarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetAll() 
        {
            GetAllAvatarQuery query = new();
            var result = await _mediator.Send(query);

            return result.Success == true ? Ok(result.Result) : BadRequest(result.Message!.Content);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromQuery] UploadAvatarCommand command)
        {
            command.Files = Request.Form.Files;
            var result = await _mediator.Send(command);

            return result.Success == true ? Ok() : BadRequest(result.Message!.Content);
        }
    }
}
