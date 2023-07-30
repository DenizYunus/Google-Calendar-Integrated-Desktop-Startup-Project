using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OctoBackend.Infrastructure.Services.Auth
{
    public class JWTHandler : IJWTHandler
    {
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParamters;
        public JWTHandler(IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _configuration = configuration;
            _tokenValidationParamters = tokenValidationParameters;
        }

        public JWTResponse GenerateToken(UserCollection user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress),
            };
            var accessTokenLifeTime = Convert.ToDouble(_configuration["Jwt:AccessTokenLifeTimeInDays"]);
            var refreshTokenLifeTime = Convert.ToDouble(_configuration["Jwt:RefreshTokenLifeTimeInDays"]);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(accessTokenLifeTime),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                JwtId = token.Id,
                ExpireDate = DateTime.UtcNow.AddSeconds(accessTokenLifeTime + refreshTokenLifeTime)
            };
            return new JWTResponse { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public static string GenerateRefreshToken()
        {
            byte[] number = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }

        public bool TryAuthenticateToken(string token, out IEnumerable<Claim>? claims)
        {
            claims = null;
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParamters, out var validatedToken);
                if (!IsJwtWithValidAlgorithm(validatedToken))
                {
                    return false;
                }
                claims = principal.Claims;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static bool IsJwtWithValidAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
