using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.IntegrationEvents;
using OctoBackend.Application.Models;


namespace OctoBackend.Application.Features.Commands.User.RemindMeQuestions
{
    public class RemindQuestionsHandler : IRequestHandler<RemindQuestionsCommand, Response>
    {
        private readonly IJWTHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IEventBus _eventBus;
        public RemindQuestionsHandler(IJWTHandler jwtHandler, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IEventBus eventBus)
        {
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _eventBus = eventBus;
        }
        public async Task<Response> Handle(RemindQuestionsCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new Response { Message = new("Inavlid token") };

            var userID = claims!.First(claim => claim.Type == "id").Value;
            var user = await _userRepository.GetByIdAsync(userID);

            QuestionReminderSetIntegrationEvent remindEvent = new()
            {
                ReminderDate = command.ReminderDate,
                EmailAddress = user.EmailAddress,
                UserName = user.UserName!
            };

            try
            {
                _eventBus.Publish(remindEvent);
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message)};
            }

            return new() { Success = true};

        }
    }
}
