using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Abstractions.Storage;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;

namespace OctoBackend.Application.Features.Commands.User.UploadCustomPhoto
{
    public class UploadCustomPhotoHandler : IRequestHandler<UploadCustomPhotoCommand, GetOneResponse<UploadCustomPhotoResponse>>
    {
        private readonly IStorage _storage; 
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IJWTHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadCustomPhotoHandler(IStorage storage, IConfiguration configuration, IUserRepository userRepository, IJWTHandler jwtHandler, IHttpContextAccessor httpContextAccessor)
        {
            _storage = storage;
            _configuration = configuration;
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetOneResponse<UploadCustomPhotoResponse>> Handle(UploadCustomPhotoCommand command, CancellationToken cancellationToken)
        {
            string token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!_jwtHandler.TryAuthenticateToken(token, out var claims))
                return new() { Message = new("Invalid token") };

            string containerName = _configuration["AzureStorage:CustomPhotosContainerName"]!;

            try
            {
              
                (string fileName, string pathOrContainerName) = await
                             _storage.UploadSingleAsync(containerName, command.File);

                List<AvatarCollection> avatars = new();
                string baseURL = _configuration["AzureStorage:BasePathURL"]!;

                var userID = claims!.First(claim => claim.Type == "id").Value;
                var user = await _userRepository.GetByIdAsync(userID);

                if (user.ProfilePictureURL != null)
                {
                    string url = user.ProfilePictureURL;
                    string oldFileName = Path.GetFileName(url);

                    if (_storage.HasFile(containerName, oldFileName))
                        await _storage.DeleteAsync(containerName, oldFileName);
                }

                user.ProfilePictureURL = baseURL + pathOrContainerName;

                await _userRepository.ReplaceOneAsync(user, userID);

                UploadCustomPhotoResponse response = new()
                {
                    ProfilePictureURL = baseURL + pathOrContainerName
                };

                return new() { Result = response, Success = true };
            }
            catch (Exception ex)
            {
                return new() {Message = new(ex.Message)};
            }
        }
    }
}
