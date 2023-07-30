using FluentValidation;
using OctoBackend.Application.Features.Commands.User.RemindMeQuestions;

namespace OctoBackend.Application.Features.Commands.User.RemindQuestions
{
    public class RemindQuestionsCommandValidator : AbstractValidator<RemindQuestionsCommand>
    {
        public RemindQuestionsCommandValidator()
        {
            RuleFor(d => d.ReminderDate)
            .GreaterThan(DateTime.Now.AddHours(+3))
            .WithMessage("Reminder date must be greater than the current date.");
        }
    }
}
