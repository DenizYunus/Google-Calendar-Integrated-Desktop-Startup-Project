using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.GoogleSignUpStart
{
    public class GoogleSignUpStartCommand : IRequest<CreateResponse<GoogleSignUpStartResponse>>
    {
        public string AuthCode { get; set; } = null!;
    }
}
