using MediatR;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.GoogleLogin
{
    public class GoogleLoginHandler : IRequestHandler<GoogleLoginCommand, CreateResponse<GoogleLoginResponse>>
    {
        private readonly IAuthService _authService;

        public GoogleLoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<CreateResponse<GoogleLoginResponse>> Handle(GoogleLoginCommand command, CancellationToken cancellationToken)
        {
            return await _authService.GoogleLoginAsync(command);
        }
    }
}
