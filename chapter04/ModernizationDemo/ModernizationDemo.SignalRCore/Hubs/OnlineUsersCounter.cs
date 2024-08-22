using Microsoft.AspNetCore.SignalR;

namespace ModernizationDemo.SignalRCore.Hubs
{
    public class OnlineUsersCounter : Hub
    {

        private static int counter = 0;

        public override async Task OnConnectedAsync()
        {
            var newCount = Interlocked.Increment(ref counter);
            await Clients.All.SendAsync("ProcessMessage", newCount);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var newCount = Interlocked.Decrement(ref counter);
            await Clients.All.SendAsync("ProcessMessage", newCount);

            await base.OnDisconnectedAsync(exception);
        }

    }
}
