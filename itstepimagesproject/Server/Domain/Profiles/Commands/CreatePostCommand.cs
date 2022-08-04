using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.Commands
{
    public class CreatePostCommand : IRequest<Unit>
    {
        public string EmailProfile { get; set; }
        public string Content { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
