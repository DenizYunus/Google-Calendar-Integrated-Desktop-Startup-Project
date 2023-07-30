using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using OctoBackend.Application.Abstractions;
using OctoBackend.Application.Abstractions.Services.Auth;
using OctoBackend.Application.Abstractions.Services.Email;
using OctoBackend.Application.Abstractions.Services.Google;
using OctoBackend.Application.Abstractions.Storage;
using OctoBackend.Domain.EventBus;
using OctoBackend.Infrastructure.EventBus.RabbitMQ;
using OctoBackend.Infrastructure.Services.Auth;
using OctoBackend.Infrastructure.Services.Email;
using OctoBackend.Infrastructure.Services.Google;
using Microsoft.Extensions.Configuration;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Microsoft.AspNetCore.Http;
using OctoBackend.Application.Abstractions.Repositories.InMemoryRepositories;
using OctoBackend.Infrastructure.Repositories;
using Microsoft.Net.Http.Headers;

namespace OctoBackend.Infrastructure.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureRegistration(this IServiceCollection services)
        {
           services.AddScoped<IJWTHandler, JWTHandler>();
           services.AddSingleton<IEmailService, EmailService>();
        }

        public static void AddGoogleRegistration(this IServiceCollection services)
        {
            services.AddScoped<IGoogleAuth, GoogleAuth>(provider =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var userCredentialRepository = provider.GetRequiredService<IDictionaryRepository<string, UserCredential>>();

                var userAgent = httpContextAccessor.HttpContext!.Request.Headers["User-Agent"].ToString();
                bool isWebAuth = false;

                if (userAgent.Contains("Mozilla"))
                    isWebAuth = true;

                     return new GoogleAuth(userCredentialRepository, configuration, isWebAuth);
            });

            _ = services.AddScoped<IGoogleCalendarService>(provider =>
            {
                var dictionaryRepository = provider.GetRequiredService<IDictionaryRepository<string, UserCredential>>();

                //TODO: Not sure to get ID from http
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var jwtHandler = provider.GetRequiredService<IJWTHandler>();
                //var userID = httpContextAccessor.HttpContext.User.FindFirstValue("id")!;

                string token = httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwtHandler.TryAuthenticateToken(token, out var claims))
                {
                    var userId = claims!.Single(x => x.Type == "id").Value;
                    return new GoogleCalendarService(dictionaryRepository, userId);
                }
                else
                {
                    return new GoogleCalendarService(dictionaryRepository, null);
                }


            });
            services.AddSingleton<IDictionaryRepository<string, UserCredential>, GoogleClientsRepository>();
        }

        public static void AddStorage<T>(this IServiceCollection services) where T : class, IStorage
        {
            services.AddScoped<IStorage, T>();
        }

        public static void AddEventBus(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSingleton<IEventBus,RabbitMQServiceBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                };

                return new RabbitMQServiceBus(config, sp, configuration);
            });
        }
    }
}
