using OctoBackend.Application.Features.Commands.Todo.Add;
using OctoBackend.Application.Features.Commands.Todo.Delete;
using OctoBackend.Application.Features.Commands.Todo.Update;
using OctoBackend.Application.Features.Queries.Todo.GetByCategory;
using OctoBackend.Application.Models;
using System.Security.Claims;

namespace OctoBackend.Application.Abstractions.Services
{
    public interface ITodoService
    {
        Task<GetOneResponse<GetTodoByCategoryResponse>> GetByCategoryAsync(GetTodoByCategoryQuery query, IEnumerable<Claim> userClaims);
        Task<CreateResponse<AddTaskResponse>> AddTaskAsync(AddTaskCommand command, IEnumerable<Claim> userClaims);
        Task<Response> SetDeadlineAsync(SetDeadlineCommand command, IEnumerable<Claim> userClaims);
        Task<Response> SetStatusAsync(SetTaskStatusCommand command, IEnumerable<Claim> userClaims);
        Task<Response> DeleteCompletedAsync(DeleteCompletedTasksComand command, IEnumerable<Claim> userClaims);

    }
}
