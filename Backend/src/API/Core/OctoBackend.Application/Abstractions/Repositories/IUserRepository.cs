using OctoBackend.Domain.Collections;

namespace OctoBackend.Application.Abstractions.Repositories
{
    public interface IUserRepository : IMongoRepository<UserCollection>
    {
        Task<ICollection<UserCollection>> GetByUserNameFilterAsync(string userNameFilter);
    }
}
