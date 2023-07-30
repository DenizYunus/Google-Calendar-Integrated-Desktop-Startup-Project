using MediatR;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace OctoBackend.Application.Features.Queries.Todo.GetByCategory
{
    public class GetTodoByCategoryHandler : IRequestHandler<GetTodoByCategoryQuery, GetOneResponse<GetTodoByCategoryResponse>>
    {
        private readonly IJWTHandler _jwtHandler;
        private readonly ITodoService _todoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTodoByCategoryHandler(ITodoService todoService, IJWTHandler jwtHandler, IHttpContextAccessor httpContextAccessor)
        {
            _todoService = todoService;
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetOneResponse<GetTodoByCategoryResponse>> Handle(GetTodoByCategoryQuery query, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new() { Message = new("Invalid Token") };

            return await _todoService.GetByCategoryAsync(query, claims!);
        }
    }
}
