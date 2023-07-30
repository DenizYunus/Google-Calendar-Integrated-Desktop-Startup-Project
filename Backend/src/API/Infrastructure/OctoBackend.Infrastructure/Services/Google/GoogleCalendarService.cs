using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using MongoDB.Bson;
using OctoBackend.Application.Abstractions.Repositories.InMemoryRepositories;
using OctoBackend.Application.Abstractions.Services.Google;
using OctoBackend.Domain.Collections;

namespace OctoBackend.Infrastructure.Services.Google
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly UserCredential? _credential;
        private CalendarService? _calendarService;
        readonly IDictionaryRepository<string, UserCredential> _userCredentials;

        public GoogleCalendarService(IDictionaryRepository<string, UserCredential> userCredentials, string? userID)
        {
            _userCredentials = userCredentials;
            if(userID is not null)
            {
                if (userCredentials.TryGetValue(userID) is null)
                    Console.WriteLine("No user exists with ID: "+ userID);

                _credential = userCredentials.TryGetValue(userID);
                _calendarService = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = _credential
                });
            }          
        }

        public async Task EnsureClientAuthorizedAsync()
        {
            if (_credential is null)
                throw new Exception("Credential is null, not created once");

            if (_credential.Token.IsExpired(SystemClock.Default))
            {
                await _credential.RefreshTokenAsync(CancellationToken.None);

                _calendarService = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = _credential
                });
            }                              
        }

        ///TODO: EventCollection is for db create should we create a new object for googleEvent???
        ///TODO: After user cancels event we even can get the event by its eventID? Reseach it
        public async Task<string> AddEventAsync(EventCollection @event)
        {
            await EnsureClientAuthorizedAsync();


            ///TODO: WILL BE FOUND A BETTER SOLUTION AFTER THIS PROJECT COMPLETES AS PROTOTYPE -> TIMEZONE DECREASED HARDCODED
            Event newEvent = new()
            {
                Id = @event.Id,
                Summary = @event.Name,
                Start = new EventDateTime { DateTime = @event.StartAt.AddHours(-3.0), TimeZone = "Europe/Istanbul" },
                End = new EventDateTime { DateTime = @event.EndAt.AddHours(-3.0), TimeZone = "Europe/Istanbul" },
                ConferenceData = new ConferenceData()
                {
                    CreateRequest = new CreateConferenceRequest()
                    {
                        RequestId = Guid.NewGuid().ToString(),
                    }
                },
            };

            EventsResource.InsertRequest request = _calendarService!.Events.Insert(newEvent, "primary");
            request.ConferenceDataVersion = 1;
            var createdEvent = await request.ExecuteAsync();

            return createdEvent.HangoutLink;
        }

        public async Task AddCollaboratorAsync(string ownerID, string eventId, string collaboratorEmail)
        {
            var credential = _userCredentials.TryGetValue(ownerID);
            _calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });

            EventsResource.GetRequest getRequest = _calendarService!.Events.Get("primary", eventId);
            Event existingEvent = await getRequest.ExecuteAsync();

            EventAttendee collaborator = new() { Email = collaboratorEmail,  };

            existingEvent.Attendees ??= new List<EventAttendee>();
            existingEvent.Attendees.Add(collaborator);

            EventsResource.UpdateRequest updateRequest = _calendarService!.Events.Update(existingEvent, "primary", eventId);
            await updateRequest.ExecuteAsync();
        }

        ///TODO: REATED CODES FOLLOW DESIGN PATTERNS IN ORDER TO NOT DRY
        public async Task<string> GenerateGoogleMeetLinkAsync()
        {
            await EnsureClientAuthorizedAsync();

            string eventID = ObjectId.GenerateNewId().ToString();
            Event newEvent = new()
            {
                Id = eventID,
                Summary = "Video Meeting",
                Start = new EventDateTime { DateTime = DateTime.Now },
                End = new EventDateTime { DateTime = DateTime.Now.AddMinutes(30)},
                ConferenceData = new ConferenceData()
                {
                    CreateRequest = new CreateConferenceRequest()
                    {
                        RequestId = Guid.NewGuid().ToString(),
                    }
                },
            };

            EventsResource.InsertRequest request = _calendarService!.Events.Insert(newEvent, "primary");
            request.ConferenceDataVersion = 1;
            var createdEvent = await request.ExecuteAsync();

            await _calendarService.Events.Delete("primary", eventID).ExecuteAsync();

            return createdEvent.HangoutLink;
        }
      
    }
}
