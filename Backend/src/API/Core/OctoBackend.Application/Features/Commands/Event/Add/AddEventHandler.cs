

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Event.Add
{
    public class AddEventHandler : IRequestHandler<AddEventCommand, Response>
    {
        private readonly IJWTHandler _jwtHandler;
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddEventHandler(IEventService eventService, IHttpContextAccessor httpContextAccessor, IJWTHandler jwtHandler)
        {
            _eventService = eventService;
            _httpContextAccessor = httpContextAccessor;
            _jwtHandler = jwtHandler;
        }

        public async Task<Response> Handle(AddEventCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new Response { Message = new("Inavlid token") };

            return await _eventService.AddAsync(command, claims!);
        }
    }
}
