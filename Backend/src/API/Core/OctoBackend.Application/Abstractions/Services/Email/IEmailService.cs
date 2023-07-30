

using OctoBackend.Application.Models;

namespace OctoBackend.Application.Abstractions.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(EmailBox box);
    }
}
