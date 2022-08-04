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
    public class GetProfileByEmailQueryHandler : IRequestHandler<GetProfileByEmailQuery, Profile>
    {
        private readonly CosmosClient cosmosClient;
        public GetProfileByEmailQueryHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }
        public async Task<Profile> Handle(GetProfileByEmailQuery request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Profiles");
            var query = $"SELECT * FROM c WHERE c.type = 'profile' AND c.email = '{request.Email}'";
            var result = container.GetItemQueryIterator<Profile>(query);
            await foreach (var item in result)
            {
                return item;
            }
            throw new Exception();
        }
    }
}
