using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;



namespace OctoBackend.Application.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(assembly);
            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddFluentValidationAutoValidation();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
