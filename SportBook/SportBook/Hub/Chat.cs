using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SportBook.ChatHub
{
    public class Chat : Hub
    {
        public void SendMessage(string name, string message)
        {
            Clients.All.SendAsync("sendMessage", name, message);
        }
        public void SendToGroup(string name, string groupName, string message)
        {
            Clients.Group(groupName).SendAsync("sendToGroup", name, message);
        }
        public override Task OnConnectedAsync()
        {
            var name = Context.GetHttpContext().Request.Query["name"];
            //return Clients.All.SendAsync("sendMessage", $"{name} joined the chat");
            return Clients.All.SendAsync("sendMessage", $"{name} joined the chat");
        }
        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    var name = Context.GetHttpContext().Request.Query["name"];
        //    return Clients.All.SendAsync("Send", $"{name} left the chat");
        //}
        // Method for testing
        public void Echo(string name, string message)
        {
            Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
        }
        public async Task JoinGroup(string name, string groupName)
        {            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //await Clients.Group(groupName).SendAsync("echo", "_SYSTEM_", $"{name} joined {groupName} with connectionId {Context.ConnectionId}");      
            // instant message to group on group join
            await Clients.Group(groupName).SendAsync("echo", "_SYSTEM_", $"{name} has joined the room");
        }
    }
}
