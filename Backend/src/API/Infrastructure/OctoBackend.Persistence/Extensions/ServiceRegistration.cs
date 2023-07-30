using Microsoft.Extensions.DependencyInjection;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Persistence.Repositories;
using OctoBackend.Persistence.Services;
using OctoBackend.Persitence.Contexts;
using OctoBackend.Persitence.Repositories;
using OctoBackend.Persitence.Services;

namespace OctoBackend.Persistence.Extension
{
    public static class ServiceRegistration
    {     
        public static void AddPersistenceRegistration(this IServiceCollection services)
        {
            services.AddSingleton<OctoDBContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAvatarRepository, AvatarRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<ITaskHistoryRepository, TaskHistoryRepository>();
        }      
    }
}
