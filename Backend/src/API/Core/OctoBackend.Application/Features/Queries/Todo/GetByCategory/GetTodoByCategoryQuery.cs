using MediatR;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Enums;

namespace OctoBackend.Application.Features.Queries.Todo.GetByCategory
{
    public class GetTodoByCategoryQuery : IRequest<GetOneResponse<GetTodoByCategoryResponse>>
    {
        public string Category { get; set; } = null!;
    }
}
