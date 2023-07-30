using OctoBackend.Domain.Collections;

namespace OctoBackend.Application.Abstractions.Repositories
{
    public interface IAvatarRepository : IMongoRepository<AvatarCollection>
    {
    }
}
