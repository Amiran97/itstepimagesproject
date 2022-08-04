﻿using Azure.Cosmos;
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
    public class ChangeProfilePhotoCommandHandler : IRequestHandler<ChangeProfilePhotoCommand, Unit>
    {
        private readonly CosmosClient cosmosClient;
        public ChangeProfilePhotoCommandHandler(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task<Unit> Handle(ChangeProfilePhotoCommand request, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer("ItStepImages", "Profiles");
            Profile profile = null;
            var query = $"SELECT * FROM c WHERE c.type = 'profile' AND c.name = '{request.Name}'";
            var result = container.GetItemQueryIterator<Profile>(query);
            await foreach (var item in result)
            {
                profile = item;
                break;
            }
            profile.Avatar = request.Photo;
            await container.ReplaceItemAsync(profile, profile.Id, new PartitionKey(profile.Id));
            return Unit.Value;
        }
    }
}
