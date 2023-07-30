using MongoDB.Bson;
using MongoDB.Driver;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Domain.Collections;
using OctoBackend.Persistence.Repositories.BaseRepository;
using OctoBackend.Persitence.Contexts;

namespace OctoBackend.Persistence.Repositories
{
    public class EventRepository : MongoRepository<EventCollection>, IEventRepository
    {
        public EventRepository(OctoDBContext context) : base(context)
        {
        }

        public async Task<ICollection<EventCollection>> GetNearestByCountAsync(int count, string userID)
        {
            var currentDateTime = DateTime.Now;

            var filterBuilder = Builders<EventCollection>.Filter;

            var filter = filterBuilder.And(
                filterBuilder.Gte(i => i.EndAt, currentDateTime),

                filterBuilder.Or(
                      filterBuilder.Eq(i => i.Owner, userID),
                      filterBuilder.AnyEq(i => i.Collaborators.Select(c => c.ID), userID)
                      )
                );


            var events = await _collection.Find(filter)
                .SortBy(i => i.StartAt)
                .Limit(count)
                .ToListAsync();

            return events;
        }

    }
}
