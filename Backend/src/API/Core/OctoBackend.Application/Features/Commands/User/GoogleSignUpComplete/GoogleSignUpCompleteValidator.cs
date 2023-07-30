
using FluentValidation;

namespace OctoBackend.Application.Features.Commands.User.GoogleSignUpComplete
{
    public class GoogleSignUpCompleteValidator : AbstractValidator<GoogleSignUpCompleteCommand>
    {
        public GoogleSignUpCompleteValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty()
                 .MinimumLength(2).WithMessage("{PropertyName} must be at least {MinLength} character")
                 .MaximumLength(20).WithMessage("{PropertyName} can be up to {MaxLength} character");

            RuleFor(n => n.Name)
                .NotEmpty()
                 .MinimumLength(2).WithMessage("{PropertyName} must be at least {MinLength} character")
                 .MaximumLength(20).WithMessage("{PropertyName} can be up to {MaxLength} character");
        }
    }
}
