
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Queries.User.GetByUserNameFilter
{
    public class GetUserByUserNameFilterHandler : IRequestHandler<GetUserByUserNameFilterQuery, GetManyResponse<GetUserByUserNameFilterResponse>>
    {
        private readonly IJWTHandler _jwtHandler;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserByUserNameFilterHandler(IUserService userService, IHttpContextAccessor httpContextAccessor, IJWTHandler jwtHandler)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _jwtHandler = jwtHandler;
        }

        public async Task<GetManyResponse<GetUserByUserNameFilterResponse>> Handle(GetUserByUserNameFilterQuery query, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claim))
                return new() { Message = new("Inavlid token") };

            return await _userService.GetByUserNameFilterAsync(query);
        }
    }
}
