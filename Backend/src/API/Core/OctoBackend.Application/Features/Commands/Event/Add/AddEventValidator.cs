
using FluentValidation;
using OctoBackend.Application.FromBodyModels.Event;

namespace OctoBackend.Application.Features.Commands.Event.Add
{
    public class AddEventValidator : AbstractValidator<AddEventBody>
    {
        public AddEventValidator()
        {

            RuleFor(d => d.EndAt)
           .GreaterThan(i => i.StartAt)
           .WithMessage("End Date must be greater than the start date.");
        }
    }
}
