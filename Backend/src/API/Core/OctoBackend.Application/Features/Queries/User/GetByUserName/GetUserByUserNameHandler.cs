
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Abstractions.Services;
using MediatR;
using OctoBackend.Application.Models;
using Microsoft.Net.Http.Headers;

namespace OctoBackend.Application.Features.Queries.User.GetByUserName
{
    
    public class GetUserByUserNameHandler : IRequestHandler<GetUserByUserNameQuery, GetOneResponse<GetUserByUserNameResponse>>
    {
        private readonly IJWTHandler _jwtHandler;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserByUserNameHandler(IUserService userService, IHttpContextAccessor httpContextAccessor, IJWTHandler jwtHandler)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _jwtHandler = jwtHandler;
        }

        public async Task<GetOneResponse<GetUserByUserNameResponse>> Handle(GetUserByUserNameQuery query, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out _))
                return new() { Message = new("Inavlid token") };

            return await _userService.GetByUserNameAsync(query);
        }
    }
}
