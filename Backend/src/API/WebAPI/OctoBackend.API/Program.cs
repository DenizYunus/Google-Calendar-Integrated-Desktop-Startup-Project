using Microsoft.OpenApi.Models;
using OctoBackend.API.Extensions;
using OctoBackend.API.IntegrationEvents.EventHandlers;
using OctoBackend.API.IntegrationEvents.Events;
using OctoBackend.Application.Extensions;
using OctoBackend.Infrastructure.Extensions;
using OctoBackend.Infrastructure.Services.Storage.Azure;
using OctoBackend.Persistence.Extension;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationRegistration();
builder.Services.AddInfrastructureRegistration();
builder.Services.AddPersistenceRegistration();

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddGoogleRegistration();

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddJwtBearerAuthentication();

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Title", Version = "v2" });
});

builder.Services.AddEventBus(builder.Configuration);
builder.Services.AddTransient<QuestionReminderSetIntegrationEventHandler>();

ConfigureSubscription.AddSubscription<QuestionReminderSetIntegrationEvent, QuestionReminderSetIntegrationEventHandler>(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
