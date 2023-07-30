using MediatR;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Models;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions.Repositories;
using AutoMapper;

namespace OctoBackend.Application.Features.Commands.User.CompleteCrispyQuestions
{
    public class CompleteCrispyQuestionsHandler : IRequestHandler<CompleteCrispyQuestionsCommand, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJWTHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompleteCrispyQuestionsHandler(IJWTHandler jwtHandler, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IMapper mapper)
        {;
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<Response> Handle(CompleteCrispyQuestionsCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new Response { Message = new("Inavlid token") };

            var userID = claims!.First(claim => claim.Type == "id").Value;

            try
            {
                var user = await _userRepository.GetByIdAsync(userID);
                _mapper.Map(command, user);


                await _userRepository.ReplaceOneAsync(user, userID);
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message)};
            }


            return new() { Success = true };
        }
    }
}
