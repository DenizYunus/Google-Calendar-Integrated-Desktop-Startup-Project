using Google.Apis.Auth.OAuth2.Flows;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Google.Apis.Auth.OAuth2.Responses;
using OctoBackend.Application.Abstractions.Services.Google;
using Google.Apis.Auth.OAuth2;
using OctoBackend.Application.Abstractions.Repositories.InMemoryRepositories;
using OctoBackend.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;

namespace OctoBackend.Infrastructure.Services.Google
{
    public class GoogleAuth : IGoogleAuth
    {
        ///TODO: CREDENTIAL COULD BE STORED AT DATABASE
        private UserCredential? _userCredential;
        protected GoogleAuthorizationCodeFlow? _authorizationCodeFlow;
        protected ValidationSettings? _validationSettings;
        protected readonly IDictionaryRepository<string, UserCredential> _userCredentialRepository;
        readonly string _redirctURI;

        public GoogleAuth(IDictionaryRepository<string, UserCredential> userCredentialRepository, IConfiguration configuration, bool isWebAuth)
        {
            _userCredentialRepository = userCredentialRepository;

            var clientIdKey = isWebAuth ? "GoogleAuth:Web:ClientID" : "GoogleAuth:Desktop:ClientID";
            var clientSecretKey = isWebAuth ? "GoogleAuth:Web:SecretKey" : "GoogleAuth:Desktop:SecretKey";
            var redirctURIKey = isWebAuth ? "GoogleAuth:Web:Redirect_URI" : "GoogleAuth:Desktop:Redirect_URI";

            var clientID = configuration[clientIdKey]!;
            var clientSecret = configuration[clientSecretKey]!;
            _redirctURI = configuration[redirctURIKey]!;

            _authorizationCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
            });

            _validationSettings = new ValidationSettings()
            {
                Audience = new List<string> { clientID }
            };
        }

        public async Task<(string, bool)> ValidateAuthCodeAsync(string authCode)
        {
            TokenResponse tokenResponse = await _authorizationCodeFlow!.ExchangeCodeForTokenAsync("user", authCode, _redirctURI, CancellationToken.None);

            var payload = await ValidateAsync(tokenResponse.IdToken, _validationSettings);

            if (!tokenResponse.Scope.Contains(GoogleConsts.GoogleCalendarURL))
                return (payload.Email, false);

            _userCredential = new(_authorizationCodeFlow, "user", tokenResponse);

           return (payload.Email, true);
        }
        public void AddClient(string userID)
        {
            if(_userCredential is null)
                throw new Exception("Credential is null, not created once");

            if (_userCredentialRepository.ContainsKey(userID))
            {
                _userCredentialRepository.Update(userID, _userCredential);
                return;
            }
            _userCredentialRepository.Add(userID, _userCredential);
        }
           
    }
}
