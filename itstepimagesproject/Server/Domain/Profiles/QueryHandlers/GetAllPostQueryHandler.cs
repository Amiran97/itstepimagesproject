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
    public class GetAllPostQueryHandler : IRequestHandler<GetAllPostQuery, IEnumerable<Post>>
    {
        private readonly CosmosClient cosmosClient;
        public GetAllPostQueryHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }
        public async Task<IEnumerable<Post>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Posts");
            var query = "SELECT * FROM c WHERE c.type = 'post'";
            var result = container.GetItemQueryIterator<Post>(query);
            var list = new List<Post>();
            await foreach (var item in result)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
