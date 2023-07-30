using AutoMapper;
using OctoBackend.Application.Features.Commands.Event.Add;
using OctoBackend.Application.Features.Commands.Event.CreateInstanceEvent;
using OctoBackend.Application.Features.Commands.Todo.Add;
using OctoBackend.Application.Features.Commands.User.CompleteCrispyQuestions;
using OctoBackend.Application.Features.Commands.User.GoogleLogin;
using OctoBackend.Application.Features.Commands.User.UpdateUser;
using OctoBackend.Application.Features.Queries.Event.GetUpcomings;
using OctoBackend.Application.Features.Queries.Todo.GetByCategory;
using OctoBackend.Application.Features.Queries.User.GetByUserName;
using OctoBackend.Application.Features.Queries.User.GetByUserNameFilter;
using OctoBackend.Application.FromBodyModels.Event;
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Models;

namespace OctoBackend.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UpdateUserCommand, UserCollection>();
            CreateMap<CompleteCrispyQuestionsCommand, UserCollection>();
            CreateMap<AddEventCommand, EventCollection>()
               .ForMember(dest => dest.Collaborators, opt => opt.Ignore());
            CreateMap<AddEventBody, AddEventCommand>();
            CreateMap<EventCollection, GetUpomingEventsResponse>();
            CreateMap<UserCollection, GetUserByUserNameFilterResponse>();
            CreateMap<UserCollection, GetUserByUserNameResponse>();
            CreateMap<UserCollection, GoogleLoginResponse>();
            CreateMap<string, CreateInstanceEventResponse>()
               .ForMember(dest => dest.GoogleMeetLink, opt => opt.MapFrom(src => src));
            CreateMap<AddTaskCommand, TodoCollection>();
            CreateMap<TodoCollection, GetTodoByCategoryResponse>();
        }
    }
}
