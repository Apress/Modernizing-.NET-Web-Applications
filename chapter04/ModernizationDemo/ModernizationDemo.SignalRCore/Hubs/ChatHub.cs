using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;

namespace ModernizationDemo.SignalRCore.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> connectionNames = new();
    
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("addChatMessage", "System", $"User {GetUserName()} joined the room.");
    }

    public async Task SendMessage(string roomId, string message)
    {
        await Clients.Group(roomId).SendAsync("addChatMessage", GetUserName(), message);
    }

    public async Task SetName(string roomId, string name)
    {
        var oldName = GetUserName();
        connectionNames[Context.ConnectionId] = name;

        await Clients.Group(roomId).SendAsync("addChatMessage", "System", $"User {oldName} changed name to {name}.");
    }

    private string GetUserName()
    {
        return connectionNames.TryGetValue(Context.ConnectionId, out var name) ? name : Context.ConnectionId;
    }
}