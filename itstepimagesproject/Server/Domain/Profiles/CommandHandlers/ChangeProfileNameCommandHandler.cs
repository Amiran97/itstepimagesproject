using Azure.Cosmos;
using itstepimagesproject.Server.Domain.Profiles.Commands;
using itstepimagesproject.Server.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.CommandHandlers
{
    public class ChangeProfileNameCommandHandler : IRequestHandler<ChangeProfileNameCommand, Unit>
    {
        private readonly CosmosClient cosmosClient;
        public ChangeProfileNameCommandHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task<Unit> Handle(ChangeProfileNameCommand request, CancellationToken cancellationToken)
        {
           var container = cosmosClient.GetContainer("ItStepImages", "Profiles");
           var profile = await container.ReadItemAsync<Profile>(request.Id, new PartitionKey(request.Id));
           profile.Value.Name = request.Name;
           await container.ReplaceItemAsync(profile.Value, request.Id, new PartitionKey(request.Id));
           return Unit.Value;
        }
    }
}
