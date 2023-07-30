using AutoMapper;
using MongoDB.Driver;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Abstractions.Services.Google;
using OctoBackend.Application.Features.Commands.User.GoogleLogin;
using OctoBackend.Application.Features.Commands.User.GoogleSignUpComplete;
using OctoBackend.Application.Features.Commands.User.GoogleSignUpStart;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Enums;
using System.Security.Claims;

namespace OctoBackend.Persitence.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTHandler _jwtHandler;
        private readonly IAvatarRepository _avatarRepository;
        private readonly IMapper _mapper;
        private readonly ITodoRepository _todoRepository;
        private readonly IGoogleAuth _googleAuth;

        public AuthService(IUserRepository userRepository, IJWTHandler jwtHandler, IAvatarRepository avatarRepository, IMapper mapper, ITodoRepository todoRepository, IGoogleAuth googleAuth)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _avatarRepository = avatarRepository;
            _mapper = mapper;
            _todoRepository = todoRepository;
            _googleAuth = googleAuth;
        }

        public async Task<CreateResponse<GoogleSignUpStartResponse>> GoogleSignUpStartAsync(GoogleSignUpStartCommand command)
        {
            UserCollection user;

            try
            {
                var result = await _googleAuth.ValidateAuthCodeAsync(command.AuthCode);

                var email = result.Item1;

                if (email is null)
                    return new() { Message = new("Email null") };

                user = await _userRepository.GetSingleAsync(i => i.EmailAddress == email);

                if (user == null)
                {
                    UserCollection newUser = new()
                    {
                        EmailAddress = email,
                    };
                    await _userRepository.InsertOneAsync(newUser);
                    user = newUser;
                }

                if (user.Role == RoleType.User)
                    return new() {Message = new("You have already completed registration", MessageCode.Conflict) };

                var jwtResult = _jwtHandler.GenerateToken(user);

                GoogleSignUpStartResponse response = new()
                {
                    AccessToken = jwtResult.AccessToken,
                };
             

                return new() { Result = response, Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new("Invalid token: " + ex.Message) };
            }
        }

        public async Task<Response> GoogleSignUpCompleteAsync(GoogleSignUpCompleteCommand command, IEnumerable<Claim> userClaims)
        {
            try
            {
                var userId = userClaims!.First(claim => claim.Type == "id").Value;
                var user = await _userRepository.GetByIdAsync(userId);

                var usernameExist = await _userRepository.GetSingleAsync(i => i.UserName == command.UserName);
                var pictureURLExist = await _avatarRepository.GetSingleAsync(i => i.Path == command.ProfilePictureURL);

                if (usernameExist != null)
                    return new() { Message = new("Username already exists", MessageCode.Conflict) };

                if (pictureURLExist == null && command.ProfilePictureURL != null)
                    return new() { Message = new("No path matches with: " + command.ProfilePictureURL, MessageCode.NotFound) };

                if (user.ProfilePictureURL is null && command.ProfilePictureURL is null)
                    return new() { Message = new("Failed to complete registration without any profile picture") };

                user.UserName = command.UserName;
                user.Name = command.Name;

                if (command.ProfilePictureURL is not null)
                    user.ProfilePictureURL = command.ProfilePictureURL;

                await _todoRepository.InitializeCategoriesAsync(userId);

                user.Role = RoleType.User;

                await _userRepository.ReplaceOneAsync(user, userId);


                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new("Invalid token: " + ex.Message) };
            }
        }

        public async Task<CreateResponse<GoogleLoginResponse>> GoogleLoginAsync(GoogleLoginCommand command)
        {        
            try
            {
                var result = await _googleAuth.ValidateAuthCodeAsync(command.AuthCode);

                if(result.Item2 == false)
                    return new() { Message = new("Authorization failed") };

                if (result.Item1 is null)
                    return new() { Message = new("Email null") };

                UserCollection user = await _userRepository.GetSingleAsync(i => i.EmailAddress == result.Item1);

                if (user is null)
                    return new() { Message = new("There is no user exists with this IDToken", MessageCode.NotFound) };

                if (user.Role == RoleType.Uncompletedregistration)
                    return new() { Message = new("You haven't complete registration yet", MessageCode.Forbidden) };


                var jwtResult = _jwtHandler.GenerateToken(user);
                _googleAuth.AddClient(user.Id);

                var response = _mapper.Map<GoogleLoginResponse>(user);

                response.AccessToken = jwtResult.AccessToken;

                return new() { Result = response, Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new("Invalid token: " + ex.Message) };
            }
        }
    }
}
