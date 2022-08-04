using Azure.Cosmos;
using Azure.Storage.Blobs;
using itstepimagesproject.Server.Domain.Profiles.Commands;
using itstepimagesproject.Server.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Domain.Profiles.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Unit>
    {
        private readonly CosmosClient cosmosClient;
        private readonly IConfiguration configuration;

        public CreatePostCommandHandler(CosmosClient cosmosClient,
            IConfiguration configuration)
        {
            this.cosmosClient = cosmosClient;
            this.configuration = configuration;
        }
        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            Profile profile = null;
            var containerProfile = cosmosClient.GetContainer("ItStepImages", "Profiles");
            var query = $"SELECT * FROM c WHERE c.type = 'profile' AND c.email = '{request.EmailProfile}'";
            var result = containerProfile.GetItemQueryIterator<Profile>(query);
            await foreach(var item in result)
            {
                profile = item;
                break;
            }
            if (profile == null) throw new Exception("Not Found!");
            var id = Guid.NewGuid().ToString();
            var post = new Post
            {
                Id = id,
                PostId = id,
                Type = "post",
                Content = request.Content,
                CreatedAt = DateTime.Now,
                CommentsCounter = 0,
                LikesCounter = 0,
                Author = new ProfilePreview
                {
                    Id = profile.Id,
                    Name = profile.Name,
                    Avatar = profile.Avatar
                },
                Tags = new List<string>(),
                Photos = new List<string>(),
                Poster = null
            };

            foreach(var file in request.Files)
            {
               var newName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
               var blobServiceClient = new BlobServiceClient(configuration["BlobStorage"]);
               var containerClient = blobServiceClient.GetBlobContainerClient("upload");
               await containerClient.UploadBlobAsync(newName, file.OpenReadStream());
               var newPhotoUrl = "https://itstepimagesstorage.blob.core.windows.net/upload/" + newName;
               post.Photos.Add(newPhotoUrl);
            }

            if (post.Photos.Count > 0) post.Poster = post.Photos[0];
            var containerPosts = cosmosClient.GetContainer("ItStepImages", "Posts");
            await containerPosts.CreateItemAsync(post, new PartitionKey(post.PostId));

            profile.PostsCounter++;
            await containerProfile.UpsertItemAsync(profile, new PartitionKey(profile.ProfileId));

            return Unit.Value;
        }
    }
}
