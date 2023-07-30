using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OctoBackend.API.Extensions
{
    public static class ConfigureAuthentication
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            var tokenValidationParamters = new TokenValidationParameters()
            {
                //ValidIssuer = configuration["Jwt:Issuer"],
                //ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(
                        configuration["Jwt:Key"]!
                        )),
                RequireExpirationTime = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //LifetimeValidator = (notBefore, expires, token, validationParamters) => expires != null && expires > DateTime.UtcNow
            };

            services.AddSingleton(tokenValidationParamters);

            _ = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = true;
                o.SaveToken = true;

                o.TokenValidationParameters = tokenValidationParamters;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Uncompletedregistration", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Uncompletedregistration");
                });

                options.AddPolicy("User", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("User");
                });

                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin");
                });
            });

            return services;

        }
    }
}
