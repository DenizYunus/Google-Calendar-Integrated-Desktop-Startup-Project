
using MediatR;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;

namespace OctoBackend.Application.Features.Queries.Avatar.GetAll
{
    public class GetAllAvatarHandler : IRequestHandler<GetAllAvatarQuery, GetManyResponse<AvatarCollection>>
    {
        private readonly IAvatarRepository _avatarRepository;

        public GetAllAvatarHandler(IAvatarRepository avatarRepository)
        {
            _avatarRepository = avatarRepository;
        }

        public async Task<GetManyResponse<AvatarCollection>> Handle(GetAllAvatarQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var avatars = await _avatarRepository.GetAllAsync();

                return new() { Success = true, Result = avatars };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }
        }
    }
}
