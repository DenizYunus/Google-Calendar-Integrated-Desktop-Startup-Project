using OctoBackend.Application.Features.Commands.User;
using OctoBackend.Application.Features.Commands.User.GoogleLogin;
using OctoBackend.Application.Features.Commands.User.GoogleSignUpComplete;
using OctoBackend.Application.Features.Commands.User.GoogleSignUpStart;
using OctoBackend.Application.Models;
using System.Security.Claims;

namespace OctoBackend.Application.Abstractions.Services.Auth
{
    public interface IAuthService
    {
        Task<CreateResponse<GoogleSignUpStartResponse>> GoogleSignUpStartAsync(GoogleSignUpStartCommand command);
        Task<Response> GoogleSignUpCompleteAsync(
            GoogleSignUpCompleteCommand command, IEnumerable<Claim> userClaims);

        Task<CreateResponse<GoogleLoginResponse>> GoogleLoginAsync(GoogleLoginCommand command);
    }
}
