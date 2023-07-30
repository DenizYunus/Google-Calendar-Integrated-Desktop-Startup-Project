using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.User.GoogleLogin
{
    public class GoogleLoginCommand : IRequest<CreateResponse<GoogleLoginResponse>>
    {
        public string AuthCode { get; set; } = null!;
    }
}
