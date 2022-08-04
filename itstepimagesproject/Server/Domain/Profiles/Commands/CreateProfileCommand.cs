using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.Commands
{
    public class CreateProfileCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
