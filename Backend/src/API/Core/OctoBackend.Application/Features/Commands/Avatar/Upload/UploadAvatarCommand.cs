using MediatR;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Models;
using System.Security.Claims;

namespace OctoBackend.Application.Features.Commands.Avatar.Upload
{
    public class UploadAvatarCommand : IRequest<Response>
    {
        public IFormFileCollection Files { get; set; } = null!;
    }
}
