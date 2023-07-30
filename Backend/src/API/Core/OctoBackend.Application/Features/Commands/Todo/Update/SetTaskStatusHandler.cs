using MediatR;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Models;
using Microsoft.Net.Http.Headers;

namespace OctoBackend.Application.Features.Commands.Todo.Update
{
    public class SetTaskStatusHandler : IRequestHandler<SetTaskStatusCommand, Response>
    {

        private readonly IJWTHandler _jwtHandler;
        private readonly ITodoService _todoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SetTaskStatusHandler(ITodoService todoService, IHttpContextAccessor httpContextAccessor, IJWTHandler jwtHandler)
        {
            _todoService = todoService;
            _httpContextAccessor = httpContextAccessor;
            _jwtHandler = jwtHandler;
        }
        public async Task<Response> Handle(SetTaskStatusCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new Response { Message = new("Inavlid token") };

            return await _todoService.SetStatusAsync(command, claims!);
        }
    }
}
