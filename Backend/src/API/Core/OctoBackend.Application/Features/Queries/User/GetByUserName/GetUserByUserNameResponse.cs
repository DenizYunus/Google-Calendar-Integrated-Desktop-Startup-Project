using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoBackend.Application.Features.Queries.User.GetByUserName
{
    public class GetUserByUserNameResponse
    {
        public string Id { get; set; } = null!;
        public string ProfilePictureURL { get; set; } = null!;
    }
}
