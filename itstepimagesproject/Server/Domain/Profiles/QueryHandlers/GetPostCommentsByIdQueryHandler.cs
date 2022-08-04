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
    public class GetPostCommentsByIdQueryHandler : IRequestHandler<GetPostCommentsByIdQuery, IEnumerable<Comment>>
    {
        private readonly CosmosClient cosmosClient;
        public GetPostCommentsByIdQueryHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task<IEnumerable<Comment>> Handle(GetPostCommentsByIdQuery request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Posts");
            var query = $"SELECT * FROM c WHERE c.type = 'comment' AND c.postId = '{request.PostId}'";
            var result = container.GetItemQueryIterator<Comment>(query);
            var list = new List<Comment>();
            await foreach (var item in result)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
