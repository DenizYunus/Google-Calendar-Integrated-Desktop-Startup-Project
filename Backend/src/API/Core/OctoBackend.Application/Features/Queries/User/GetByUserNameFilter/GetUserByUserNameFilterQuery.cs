using MediatR;
using OctoBackend.Application.Models;

namespace OctoBackend.Application.Features.Queries.User.GetByUserNameFilter
{
    public class GetUserByUserNameFilterQuery : IRequest<GetManyResponse<GetUserByUserNameFilterResponse>>
    {
        public string UserNameFilter { get; set; } = null!;
    }
}
