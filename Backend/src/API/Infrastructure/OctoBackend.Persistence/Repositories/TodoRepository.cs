using MongoDB.Driver;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Enums;
using OctoBackend.Domain.Models;
using OctoBackend.Persistence.Repositories.BaseRepository;
using OctoBackend.Persitence.Contexts;

namespace OctoBackend.Persistence.Repositories
{
    public class TodoRepository : MongoRepository<TodoCollection>, ITodoRepository
    {
        public TodoRepository(OctoDBContext context) : base(context)
        {
        }

        public async Task InitializeCategoriesAsync(string userID)
        {
            List<TodoCollection> initialTodos = new()
            {
                new TodoCollection()
                {
                    Owner = userID,
                    Category = TodoCategory.Personal,
                },

                 new TodoCollection()
                 {
                    Owner = userID,
                    Category = TodoCategory.Education,
                 },

                  new TodoCollection()
                  {
                    Owner = userID,
                    Category = TodoCategory.Work,
                  },
            };

            await InsertManyAsync(initialTodos);
        }
        public async Task<TodoCollection> GetByCategoryAsync(string category, string userID)
        {
            var todo = await _collection.Find(i => i.Category.ToString() == category && i.Owner == userID).FirstOrDefaultAsync();

            if (todo is null)
                return null!;

             return todo;
        }

        //public async Task<TaskTodo> GetTaskByIDAsync(string category, string userID, string taskID)
        //{
        //     var todo = await GetByCategoryAsync(category, userID);

        //     var task = todo.Tasks.FirstOrDefault(i => i.ID == taskID);

        //    if (task is null)
        //        return null!;

        //    return task;
        //}
    }
}
