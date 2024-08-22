using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ModernizationDemo.SignalR.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {

        private static readonly ConcurrentDictionary<string, string> connectionNames = new();
        
        public async Task JoinRoom(string roomId)
        {
            await Groups.Add(Context.ConnectionId, roomId);
            await Clients.Group(roomId).addChatMessage("System", $"User {GetUserName()} joined the room.");
        }

        public async Task SendMessage(string roomId, string message)
        {
            await Clients.Group(roomId).addChatMessage(GetUserName(), message);
        }

        public async Task SetName(string roomId, string name)
        {
            var oldName = GetUserName();
            connectionNames[Context.ConnectionId] = name;

            await Clients.Group(roomId).addChatMessage("System", $"User {oldName} changed name to {name}.");
        }

        private string GetUserName()
        {
            return connectionNames.TryGetValue(Context.ConnectionId, out var name) ? name : Context.ConnectionId;
        }
    }
}