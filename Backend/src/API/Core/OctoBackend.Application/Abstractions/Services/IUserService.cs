using OctoBackend.Application.Features.Commands.User.UpdateUser;
using OctoBackend.Application.Features.Queries.User.GetByUserName;
using OctoBackend.Application.Features.Queries.User.GetByUserNameFilter;
using OctoBackend.Application.Models;
using System.Security.Claims;

namespace OctoBackend.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<Response> UpdateAsync(UpdateUserCommand command, IEnumerable<Claim> userClaims);
        Task<GetManyResponse<GetUserByUserNameFilterResponse>> GetByUserNameFilterAsync(GetUserByUserNameFilterQuery query);

        Task<GetOneResponse<GetUserByUserNameResponse>> GetByUserNameAsync(GetUserByUserNameQuery query);
    }
}
