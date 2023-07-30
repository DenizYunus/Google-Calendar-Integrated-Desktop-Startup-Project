using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OctoBackend.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddJwtBearerAuthentication(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        }
    }
}
