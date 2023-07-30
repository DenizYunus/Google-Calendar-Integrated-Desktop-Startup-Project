using MediatR;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.GoogleSignUpStart
{
    public class GoogleSignUpStartHandler : IRequestHandler<GoogleSignUpStartCommand, CreateResponse<GoogleSignUpStartResponse>>
    {
        private readonly IAuthService _authService;

        public GoogleSignUpStartHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<CreateResponse<GoogleSignUpStartResponse>> Handle(GoogleSignUpStartCommand command, CancellationToken cancellationToken)
        {
           return await _authService.GoogleSignUpStartAsync(command);
        }
    }
}
