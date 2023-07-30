
namespace OctoBackend.Application.Abstractions.Services.Google
{
    public interface IGoogleAuth
    {
        Task<(string, bool)> ValidateAuthCodeAsync(string authCode);
        void AddClient(string userID);
    }
}
