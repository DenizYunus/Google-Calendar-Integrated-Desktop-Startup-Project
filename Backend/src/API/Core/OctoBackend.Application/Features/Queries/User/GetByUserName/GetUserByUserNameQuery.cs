using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Queries.User.GetByUserName
{
    public class GetUserByUserNameQuery : IRequest<GetOneResponse<GetUserByUserNameResponse>>
    {
        public string UserName { get; set; } = null!;
    }
}
