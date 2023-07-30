using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Commands.Event.CreateInstanceEvent
{
    ///TODO: NOT SURE TO CREATE 2 DIFFERENT FUNCTIONS BUT POSSIBLE TO INTEGRATE IN ONE FUNCTION
    public class CreateInstanceEventCommand : IRequest<GetOneResponse<CreateInstanceEventResponse>>
    {
    }
}
