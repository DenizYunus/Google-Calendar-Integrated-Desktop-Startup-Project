using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;


namespace OctoBackend.Application.Features.Queries.Event.GetUpcomings
{
    public class GetUpcomingEventsHandler : IRequestHandler<GetUpcomingEventsQuery, GetManyResponse<GetUpomingEventsResponse>>
    {
        private readonly IJWTHandler _jwtHandler;
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUpcomingEventsHandler(IEventService eventService, IJWTHandler jwtHandler, IHttpContextAccessor httpContextAccessor)
        {
            _eventService = eventService;
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetManyResponse<GetUpomingEventsResponse>> Handle(GetUpcomingEventsQuery command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new() { Message = new("Invalid Token") };

            return await _eventService.GetUpcomingsAsync(command, claims!);
        }
    }
}
