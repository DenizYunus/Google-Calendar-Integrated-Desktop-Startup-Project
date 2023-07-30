using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Domain.Collections;
using OctoBackend.Persistence.Repositories.BaseRepository;
using OctoBackend.Persitence.Contexts;

namespace OctoBackend.Persitence.Repositories
{
    public class AvatarRepository : MongoRepository<AvatarCollection>, IAvatarRepository
    {
        public AvatarRepository(OctoDBContext context) : base(context)
        {
        }
    }
}
