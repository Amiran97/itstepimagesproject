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
    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Unit>
    {
        private readonly CosmosClient cosmosClient;
        public CreateProfileCommandHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task<Unit> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid().ToString();

            Profile newProfile = new Profile
            {
                Name = request.Name,
                PostsCounter = 0,
                SubscribersCounter = 0,
                SubscriptionsCounter = 0,
                Avatar = "image.jpg",
                SmallImage = "image.jpg",
                Type = "profile",
                Id = id,
                ProfileId = id
            };

            var container = cosmosClient.GetContainer("ItStepImages", "Profiles");
            await container.CreateItemAsync(newProfile);

            //dynamic proceduteParams = new {
            //    name = "Ivan99",
            //    avater = "testavatar.png",
            //    smallImage = "testimage.png"
            //};
            //container.Scripts.ExecuteStoredProcedureAsync("ChangeProfilePhoto", new PartitionKey("1"), proceduteParams);
            return Unit.Value;
        }
    }
}
