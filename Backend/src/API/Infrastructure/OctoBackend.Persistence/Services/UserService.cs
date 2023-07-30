using AutoMapper;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Features.Commands.User.UpdateUser;
using OctoBackend.Application.Features.Queries.User.GetByUserName;
using OctoBackend.Application.Features.Queries.User.GetByUserNameFilter;
using OctoBackend.Application.Models;
using System.Security.Claims;

namespace OctoBackend.Persitence.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAvatarRepository _avatarRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IAvatarRepository avatarRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _avatarRepository = avatarRepository;
            _mapper = mapper;
        }

        public async Task<Response> UpdateAsync(UpdateUserCommand command, IEnumerable<Claim> userClaims)
        {
            var userID = userClaims!.First(claim => claim.Type == "id").Value;
            var user = await _userRepository.GetByIdAsync(userID);

            var usernameExist = await _userRepository.GetSingleAsync(i => i.UserName == command.UserName);
            if (usernameExist != null && command.UserName != null)
                return new() { Message = new("Username already exists", MessageCode.Conflict) };

            var pictureURLExist = await _avatarRepository.GetSingleAsync(i => i.Path == command.ProfilePictureURL);
            if (pictureURLExist == null && command.ProfilePictureURL != null)
                return new() { Message = new("No path matches with: " + command.ProfilePictureURL, MessageCode.NotFound) };

            _mapper.Map(command, user);

            await _userRepository.ReplaceOneAsync(user, userID);

            return new() { Success = true };
        }

        public async Task<GetManyResponse<GetUserByUserNameFilterResponse>> GetByUserNameFilterAsync(GetUserByUserNameFilterQuery command)
        {
            try
            {
                var users = await _userRepository.GetByUserNameFilterAsync(command.UserNameFilter);

                var responseResult = _mapper.Map<ICollection<GetUserByUserNameFilterResponse>>(users);

                return new() { Result = responseResult, Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }

        }

        public async Task<GetOneResponse<GetUserByUserNameResponse>> GetByUserNameAsync(GetUserByUserNameQuery query)
        {
            try
            {
                var user = await _userRepository.GetSingleAsync(i => i.UserName == query.UserName);

                if(user is null)
                    return new() { Message = new("No user exists with this username: " + query.UserName, MessageCode.NotFound) };

                var responseResult = _mapper.Map<GetUserByUserNameResponse>(user);

                return new() { Result = responseResult, Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };                
            }
        }
    }
}
