using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.CompleteCrispyQuestions
{
    public class CompleteCrispyQuestionsCommand : IRequest<Response>
    {
        public string Industry { get; set; } = null!;
        public string EntrepreneurField { get; set; } = null!;
        public int WorkingHoursInADay { get; set; }
        public DateTime Birthday { get; set; } //JSON atayım {"day":12, month:15, year:2001} 
        public ICollection<string> RequestedServices { get; set; } = null!;
        public ICollection<string> RequestedCollaborations { get; set; } = null!;
    }
}
