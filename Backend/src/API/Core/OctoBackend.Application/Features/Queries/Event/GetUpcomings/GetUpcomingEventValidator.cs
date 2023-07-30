using FluentValidation;

namespace OctoBackend.Application.Features.Queries.Event.GetUpcomings
{
    public class GetUpcomingEventValidator : AbstractValidator<GetUpcomingEventsQuery>
    {
        public GetUpcomingEventValidator()
        {
            RuleFor(d => d.EventCount)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0");
        }
    }
}
