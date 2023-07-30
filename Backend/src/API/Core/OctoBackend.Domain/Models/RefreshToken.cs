
namespace OctoBackend.Domain.Models
{
    public class RefreshToken
    {
        public string? Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string JwtId { get; set; } = string.Empty;
    }
}
