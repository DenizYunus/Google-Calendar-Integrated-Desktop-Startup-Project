using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Response>
    {
        private readonly IUserService _userService;
        private readonly IJWTHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateUserHandler(IUserService userService, IJWTHandler jwtHandler, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new Response { Message = new("Inavlid token") };

            return await _userService.UpdateAsync(command, claims!);
        }
    }
}
