using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.Commands
{
    public class ChangeProfilePhotoCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
