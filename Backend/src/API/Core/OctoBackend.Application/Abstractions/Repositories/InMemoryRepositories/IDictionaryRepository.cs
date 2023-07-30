

namespace OctoBackend.Application.Abstractions.Repositories.InMemoryRepositories
{
    public interface IDictionaryRepository<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        void Remove(TKey key);
        bool ContainsKey(TKey key);
        TValue TryGetValue(TKey key);
        void Update(string key, TValue updatedValue);
    }
}
