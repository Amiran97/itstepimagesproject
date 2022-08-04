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
    public class GetAllProfileQueryHandler : IRequestHandler<GetAllProfileQuery, IEnumerable<Profile>>
    {
        private readonly CosmosClient cosmosClient;
        public GetAllProfileQueryHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task<IEnumerable<Profile>> Handle(GetAllProfileQuery request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Profiles");
            var query = "SELECT * FROM c WHERE c.type = 'profile'";
            var result = container.GetItemQueryIterator<Profile>(query);
            var list = new List<Profile>();
            await foreach(var item in result)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
