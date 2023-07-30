using MediatR;
using Microsoft.Extensions.Configuration;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Storage;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;
using System.Collections.Generic;

namespace OctoBackend.Application.Features.Commands.Avatar.Upload
{
    public class UploadAvatarHandler : IRequestHandler<UploadAvatarCommand, Response>
    {
        private readonly IStorage _storage;
        private readonly IAvatarRepository _avatarRepository;
        private readonly IConfiguration _configuration;

        public UploadAvatarHandler(IStorage storage, IAvatarRepository avatarRepository, IConfiguration configuration)
        {
            _storage = storage;
            _avatarRepository = avatarRepository;
            _configuration = configuration;
        }

        public async Task<Response> Handle(UploadAvatarCommand command, CancellationToken cancellationToken)
        {
            try
            {
                List<(string fileName, string pathOrContainerName)> result = await
                        _storage.UploadAsync(_configuration["AzureStorage:AvatarsContainerName"]!, command.Files);

                List<AvatarCollection> avatars = new();
                string baseURL = _configuration["AzureStorage:BasePathURL"]!;

                foreach (var (fileName, pathOrContainerName) in result)
                {
                    AvatarCollection avatar = new()
                    {
                        FileName = fileName,
                        Path = baseURL + pathOrContainerName
                    };

                    avatars.Add(avatar);
                }
                await _avatarRepository.InsertManyAsync(avatars);

                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }       
        }
    }
}
