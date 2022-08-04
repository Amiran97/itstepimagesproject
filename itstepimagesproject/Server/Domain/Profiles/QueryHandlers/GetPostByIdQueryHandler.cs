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
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Post>
    {
        private readonly CosmosClient cosmosClient;
        public GetPostByIdQueryHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }
        public async Task<Post> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Posts");
            var item = await container.ReadItemAsync<Post>(request.PostId, new PartitionKey(request.PostId));
            return item.Value;
        }
    }
}
