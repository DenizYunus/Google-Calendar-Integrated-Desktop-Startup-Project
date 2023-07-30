

using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.UpdateUser
{
    public class UpdateUserCommand : IRequest<Response>
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? ProfilePictureURL { get; set; }
        public string? Industry { get; set; }
        public string? EntrepreneurField { get; set; }
        public int? WorkingHoursInADay { get; set; }
    }
}
