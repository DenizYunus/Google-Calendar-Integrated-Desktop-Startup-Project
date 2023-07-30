using MediatR;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Models;
using Microsoft.Net.Http.Headers;

namespace OctoBackend.Application.Features.Commands.Event.CreateInstanceEvent
{
    public class CreateInstanceEventHandler : IRequestHandler<CreateInstanceEventCommand, GetOneResponse<CreateInstanceEventResponse>>
    {
        private readonly IJWTHandler _jwtHandler;
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateInstanceEventHandler(IEventService eventService, IHttpContextAccessor httpContextAccessor, IJWTHandler jwtHandler)
        {
            _eventService = eventService;
            _httpContextAccessor = httpContextAccessor;
            _jwtHandler = jwtHandler;
        }

        public async Task<GetOneResponse<CreateInstanceEventResponse>> Handle(CreateInstanceEventCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out _))
                return new() { Message = new("Inavlid token") };

            return await _eventService.CreateInstanceEventAsync(command);
        }
    }
}
