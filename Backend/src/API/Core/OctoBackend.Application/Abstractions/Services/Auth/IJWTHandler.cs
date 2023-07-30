using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Models;
using System.Security.Claims;

namespace OctoBackend.Application.Abstractions.Services.Auth
{
    public interface IJWTHandler
    {
        public JWTResponse GenerateToken(UserCollection user);
        public bool TryAuthenticateToken(string token, out IEnumerable<Claim>? claims);
    }
}
