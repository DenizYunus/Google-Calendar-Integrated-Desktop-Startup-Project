using Google.Apis.Auth.OAuth2;
using OctoBackend.Application.Abstractions.Repositories.InMemoryRepositories;

namespace OctoBackend.Infrastructure.Repositories
{
    public class GoogleClientsRepository : IDictionaryRepository<string, UserCredential>
    {
        readonly Dictionary<string, UserCredential> _userCredentials;

        public GoogleClientsRepository()
        {
            _userCredentials = new();
        }

        public void Add(string key, UserCredential value)
            => _userCredentials.Add(key, value);

        public bool ContainsKey(string key)
            => _userCredentials.ContainsKey(key);

        public void Remove(string key)
            => _userCredentials.Remove(key);

        public UserCredential TryGetValue(string key)
        {
            if(_userCredentials.TryGetValue(key, out UserCredential? value))
                return value;

            return null!;

        }
        public void Update(string key, UserCredential updatedValue)
        {
            if (_userCredentials.ContainsKey(key))           
                _userCredentials[key] = updatedValue;           
            else
            {

                throw new KeyNotFoundException("The specified key was not found.");
            }
        }
    }
}
