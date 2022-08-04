using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itstepimagesproject.Server.Hubs
{
    public class CommentsHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Clients.Caller} connected!");
            return base.OnConnectedAsync();
        }

        public async Task NewComment(string postId, string profileId, string message)
        {
            //await Clients.All.SendAsync("NewCommentAdded", postId, profileId, message);
            await Clients.Group(postId).SendAsync("NewCommentAdded", postId, profileId, message);
        }

        public async Task JoinGroup(string postId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, postId);
        }

        public async Task LeaveGroup(string postId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, postId);
        }
    }
}
