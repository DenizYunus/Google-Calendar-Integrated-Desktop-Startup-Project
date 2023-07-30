

using MediatR;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;
using System.Security.Claims;

namespace OctoBackend.Application.Features.Queries.Avatar.GetAll
{
    public class GetAllAvatarQuery : IRequest<GetManyResponse<AvatarCollection>>
    {

    }
}
