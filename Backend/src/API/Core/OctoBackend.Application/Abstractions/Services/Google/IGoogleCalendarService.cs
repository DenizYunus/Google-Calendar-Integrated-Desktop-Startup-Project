
using OctoBackend.Domain.Collections;

namespace OctoBackend.Application.Abstractions.Services.Google
{
    public interface IGoogleCalendarService
    {
        Task EnsureClientAuthorizedAsync();
        Task<string> AddEventAsync(EventCollection @event);
        Task AddCollaboratorAsync(string ownerID, string eventId, string collaboratorEmail);
        Task<string> GenerateGoogleMeetLinkAsync();
    }
}
