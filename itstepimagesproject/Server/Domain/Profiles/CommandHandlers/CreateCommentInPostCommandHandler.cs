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
    public class CreateCommentInPostCommandHandler : IRequestHandler<CreateCommentInPostCommand, Unit>
    {
        private readonly CosmosClient cosmosClient;
        public CreateCommentInPostCommandHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }
        public async Task<Unit> Handle(CreateCommentInPostCommand request, CancellationToken cancellationToken)
        {
            var containerPost = cosmosClient.GetContainer("ItStepImages", "Posts");
            var containerProfile = cosmosClient.GetContainer("ItStepImages", "Profiles");
            Profile profile = null;
            string query = $"SELECT * FROM c WHERE c.type = 'profile' AND c.email = '{request.ProfileEmail}'";
            var result = containerProfile.GetItemQueryIterator<Profile>(query);
            await foreach(var item in result)
            {
                profile = item;
            }
            if (profile == null) throw new Exception("Profile not found!");
            var newComment = new Comment {
                Id = Guid.NewGuid().ToString(),
                PostId = request.PostId,
                Message = request.Message,
                CreatedAt = DateTime.Now,
                Type = "comment",
                Author = new ProfilePreview
                {
                    Id = profile.Id,
                    Avatar = profile.Avatar,
                    Name = profile.Name
                }
            };
            await containerPost.CreateItemAsync<Comment>(newComment, new PartitionKey(newComment.PostId));
            var post = await containerPost.ReadItemAsync<Post>(request.PostId, new PartitionKey(request.PostId));
            post.Value.CommentsCounter++;
            await containerPost.UpsertItemAsync<Post>(post.Value, new PartitionKey(post.Value.PostId));
            return Unit.Value;
        }
    }
}
