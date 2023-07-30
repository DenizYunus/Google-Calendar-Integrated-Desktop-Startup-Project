using MediatR;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;

namespace OctoBackend.Application.Features.Queries.Event.GetUpcomings
{
    public class GetUpcomingEventsQuery : IRequest<GetManyResponse<GetUpomingEventsResponse>>
    {
        public int EventCount { get; set; }
    }
}
