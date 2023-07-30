
namespace OctoBackend.Domain.Models
{
    public class JWTResponse
    {
        public string AccessToken { get; set; } = null!;
        public RefreshToken? RefreshToken { get; set; }
    }
}

