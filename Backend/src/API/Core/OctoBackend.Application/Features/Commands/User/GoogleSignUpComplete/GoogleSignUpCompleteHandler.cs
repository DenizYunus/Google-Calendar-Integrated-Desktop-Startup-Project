

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.GoogleSignUpComplete
{
    public class GoogleSignUpCompleteHandler : IRequestHandler<GoogleSignUpCompleteCommand, Response>
    {
        private readonly IAuthService _authService;
        private readonly IJWTHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GoogleSignUpCompleteHandler(IAuthService authService, IJWTHandler jwtHandler, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Response> Handle(GoogleSignUpCompleteCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new Response { Message = new("Inavlid token") };

            return await _authService.GoogleSignUpCompleteAsync(command, claims!);
        }
    }
}
