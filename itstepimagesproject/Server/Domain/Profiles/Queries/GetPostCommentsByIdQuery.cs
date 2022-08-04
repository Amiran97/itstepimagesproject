using itstepimagesproject.Server.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.Queries
{
    public class GetPostCommentsByIdQuery : IRequest<IEnumerable<Comment>>
    {
        public string PostId { get; set; }
    }
}
