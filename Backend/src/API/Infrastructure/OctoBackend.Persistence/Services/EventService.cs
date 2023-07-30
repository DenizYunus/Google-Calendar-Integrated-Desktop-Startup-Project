using AutoMapper;
using Microsoft.Extensions.Configuration;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Abstractions.Services.Email;
using OctoBackend.Application.Abstractions.Services.Google;
using OctoBackend.Application.Features.Commands.Event.Add;
using OctoBackend.Application.Features.Commands.Event.CollaboratorApproval;
using OctoBackend.Application.Features.Commands.Event.CreateInstanceEvent;
using OctoBackend.Application.Features.Queries.Event.GetUpcomings;
using OctoBackend.Application.Helpers;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Enums;
using OctoBackend.Domain.Models;
using System.Security.Claims;


namespace OctoBackend.Persistence.Services
{
    public class EventService : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;


        public EventService(IMapper mapper, IUserRepository userRepository, IEventRepository eventRepository, IGoogleCalendarService googleCalendarService, IEmailService emailService, IConfiguration configuration)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _googleCalendarService = googleCalendarService;
            _emailService = emailService;
            _configuration = configuration;
        }
        ///TODO: USE GOOGLE CALENDAR SERVİCE AT COMMAND HANDLERS 
        public async Task<Response> AddAsync(AddEventCommand command, IEnumerable<Claim> userClaims)
        {
            var userId = userClaims!.Single(x => x.Type == "id").Value;

            var dbEvent = _mapper.Map<EventCollection>(command);
            dbEvent.Owner = userId;

            List<Email> emails = new();
            string htmlText = DirectoryHelper.ReadDirectoryContent(_configuration["Email:RemindQuestionsHtml"]!);
            if (command.Collaborators.Any())
            {
                foreach (var collaboratorID in command.Collaborators)
                {
                    var collaborator = await _userRepository.GetByIdAsync(collaboratorID);

                    if (collaborator is null)
                        return new() { Message = new("There is no user with id:" + collaboratorID, MessageCode.NotFound) };

                    string token = TokenHelper.CreateToken();
                    EventCollaborator eventCollaborator = new(collaboratorID, token);                

                    dbEvent.Collaborators.Add(eventCollaborator);
                    string body = htmlText.Replace("<@name>", collaborator.UserName).Replace("<@ownerID>", userId).Replace("<@approvalToken>", token).Replace("<@eventID>", dbEvent.Id);

                    Email email = new()
                    {
                        Body = body,
                        EmailAdress = collaborator.EmailAddress
                    };
                    
                    emails.Add(email);
                }
            }
            try
            {
                string meetLink = await _googleCalendarService.AddEventAsync(dbEvent);
                dbEvent.MeetingLink = meetLink;
                await _eventRepository.InsertOneAsync(dbEvent);

                if(command.Collaborators.Any())
                {
                    
                    EmailBox emailBox = new(emails, "Event Approval");
                    await _emailService.SendAsync(emailBox);
                }
                  
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }

            return new() { Success = true };

        }

        public async Task<Response> ApproveEventAsCollaboratorAsync(CollaboratorApprovalCommand command)
        {
            var @event = await _eventRepository.GetSingleAsync(i => i.Id == command.EventID);

            if (@event == null)
                return new Response { Message = new("No event found with ID: " + command.EventID, MessageCode.NotFound) };

            var matchedCollaborator = @event.Collaborators.FirstOrDefault(c => c.ApprovalToken == command.ApprovalToken);

            if (matchedCollaborator == null)
                return new Response { Message = new("Invalid token",MessageCode.NotFound)};

            if (matchedCollaborator.ApprovalStatus != ApprovalStatus.Awaiting)
                return new Response { Message = new("Approval process already completed") };

            if (matchedCollaborator.ApprovalTokenExpireDate < DateTime.UtcNow)
                return new Response { Message = new("Token has been expired") };

            matchedCollaborator.ApprovalStatus = ApprovalStatus.Approved;
            matchedCollaborator.ApprovalToken = null;
            matchedCollaborator.ApprovalTokenExpireDate = null;

            try
            {

                var collaborator = await _userRepository.GetByIdAsync(matchedCollaborator.ID);

                await _eventRepository.ReplaceOneAsync(@event, @event.Id);
                await _googleCalendarService.AddCollaboratorAsync(@event.Owner, @event.Id, collaborator.EmailAddress);
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }
           

            return new Response {Success = true};

        }

        public async Task<GetOneResponse<CreateInstanceEventResponse>> CreateInstanceEventAsync(CreateInstanceEventCommand command)
        {
            try
            {
                var link = await _googleCalendarService.GenerateGoogleMeetLinkAsync();

                var responseResult = _mapper.Map<CreateInstanceEventResponse>(link);

                return new() { Result = responseResult, Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }

        }

        public async Task<GetManyResponse<GetUpomingEventsResponse>> GetUpcomingsAsync(GetUpcomingEventsQuery command, IEnumerable<Claim> userClaims)
        {
            var userId = userClaims!.Single(x => x.Type == "id").Value;

            var upcomingEvents = await _eventRepository.GetNearestByCountAsync(command.EventCount, userId);

            var response = _mapper.Map<ICollection<GetUpomingEventsResponse>>(upcomingEvents);

            return new() { Result = response, Success = true };
        }
    }
}
