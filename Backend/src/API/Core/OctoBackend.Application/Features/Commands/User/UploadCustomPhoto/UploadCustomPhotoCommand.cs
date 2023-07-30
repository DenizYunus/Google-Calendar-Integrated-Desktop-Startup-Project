using MediatR;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Models;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace OctoBackend.Application.Features.Commands.User.UploadCustomPhoto
{
    public class UploadCustomPhotoCommand : IRequest<GetOneResponse<UploadCustomPhotoResponse>>
    {
        public IFormFile File { get; set; } = null!;
 
    }
}
