using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.RemindMeQuestions
{
    public class RemindQuestionsCommand : IRequest<Response>
    {
        public DateTime ReminderDate { get; set; } 
    }
}
