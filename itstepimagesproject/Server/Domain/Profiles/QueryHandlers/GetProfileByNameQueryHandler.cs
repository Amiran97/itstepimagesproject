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
    public class GetProfileByNameQueryHandler : IRequestHandler<GetProfileByNameQuery, Profile>
    {
        private readonly CosmosClient cosmosClient;
        public GetProfileByNameQueryHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task<Profile> Handle(GetProfileByNameQuery request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Profiles");
            var query = $"SELECT * FROM c WHERE c.type = 'profile' AND c.name = '{request.Name}'";
            var result = container.GetItemQueryIterator<Profile>(query);
            await foreach(var item in result)
            {
                return item;
            }
            throw new Exception();
        }
    }
}
