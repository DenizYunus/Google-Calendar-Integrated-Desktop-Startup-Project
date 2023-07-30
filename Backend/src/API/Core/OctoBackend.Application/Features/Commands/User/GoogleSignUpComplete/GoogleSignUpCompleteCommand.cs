using MediatR;
using OctoBackend.Application.Models;


namespace OctoBackend.Application.Features.Commands.User.GoogleSignUpComplete
{
    public class GoogleSignUpCompleteCommand : IRequest<Response>
    {
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!; 
        public string? ProfilePictureURL { get; set; }
    }
}
