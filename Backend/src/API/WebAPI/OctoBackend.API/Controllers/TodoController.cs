using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctoBackend.API.Attributes;
using OctoBackend.Application.Features.Commands.Todo.Add;
using OctoBackend.Application.Features.Commands.Todo.Delete;
using OctoBackend.Application.Features.Commands.Todo.Update;
using OctoBackend.Application.Features.Queries.Todo.GetByCategory;
using OctoBackend.Application.FromBodyModels.Todo;
using OctoBackend.Application.Models;

namespace OctoBackend.API.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize("User")]
        [HttpGet("getbycategory")]
        public async Task<IActionResult> GetByCategory([FromQuery][ValidateTodoCategory] string category)
        {
            GetTodoByCategoryQuery query = new()
            {
                Category = category
            };

            var result = await _mediator.Send(query);

            if (result.Success)
                return Ok(result.Result);

            return BadRequest(result.Message!.Content);
        }
        ///TODO: STATUS CODE CREATED CAN BE IMPLENTED AT NEEDED ENDPOINTS

        [Authorize("User")]
        [HttpPost("add-task")]
        public async Task<IActionResult> AddTask([FromQuery][ValidateTodoCategory] string category, [FromBody] AddTaskBody body)
        {
            AddTaskCommand command = new()
            {
                Category = category,
                Task = body.Task
            };

            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok(result.Result!.ID);

            return BadRequest(result.Message!.Content);
        }

        [Authorize("User")]
        [HttpPost("set-status/{taskID}")]
        public async Task<IActionResult> SetStatus([FromRoute][ValidateObjectID] string taskID, [FromQuery][ValidateTodoCategory] string category, [FromQuery][ValidateTaskStatus] string status)
        {
            SetTaskStatusCommand command = new()
            {
                Category = category,
                TaskID = taskID,
                Status = status
            };

            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok();

            Message message = result.Message!;

            if (message.Code == MessageCode.NotFound)
                return NotFound(message.Content);


            return BadRequest(message.Content);
        }

        [Authorize("User")]
        [HttpPost("set-deadline")]
        public async Task<IActionResult> SetDeadline([ValidateTodoCategory][FromQuery] string category, [FromBody] SetDeadlineBody body)
        {
            SetDeadlineCommand command = new()
            {
                Category = category,
                Deadline = body.Deadline
            };

            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok();

            Message message = result.Message!;

            return BadRequest(message.Content);
        }

        [Authorize("User")]
        [HttpDelete("delete-completed")]
        public async Task<IActionResult> DeleteCompleted([FromQuery] DeleteCompletedTasksComand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok();

            Message message = result.Message!;

            if (message.Code == MessageCode.NotFound)
                return NotFound(message.Content);

            return BadRequest(message.Content);
        }
    }
}
