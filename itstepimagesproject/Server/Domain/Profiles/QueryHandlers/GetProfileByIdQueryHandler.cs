using Azure.Cosmos;
using itstepimagesproject.Server.Domain.Profiles.Queries;
using itstepimagesproject.Server.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.QueryHandlers
{
    public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, Profile>
    {
        private readonly CosmosClient cosmosClient;
        public GetProfileByIdQueryHandler(CosmosClient cosmosClient) 
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task<Profile> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Profiles");
            return await container.ReadItemAsync<Profile>(request.Id, new PartitionKey(request.Id));
        }
    }
}
