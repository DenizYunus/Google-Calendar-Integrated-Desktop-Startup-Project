using System.Security.Cryptography;


namespace OctoBackend.Application.Helpers
{
    public class TokenHelper
    {
        public static string CreateToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
