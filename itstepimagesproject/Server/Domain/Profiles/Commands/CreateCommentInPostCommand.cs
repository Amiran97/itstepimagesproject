using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.Commands
{
    public class CreateCommentInPostCommand : IRequest<Unit>
    {
        public string PostId { get; set; }
        public string ProfileEmail { get; set; }
        public string Message { get; set; }
    }
}
