using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.Commands
{
    public class ChangeProfileNameCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
