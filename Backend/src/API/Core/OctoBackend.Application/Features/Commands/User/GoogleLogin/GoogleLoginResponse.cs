
namespace OctoBackend.Application.Features.Commands.User.GoogleLogin
{
    public class GoogleLoginResponse
    {
        public string EmailAddress { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string ProfilePictureURL { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; }
    }
}
