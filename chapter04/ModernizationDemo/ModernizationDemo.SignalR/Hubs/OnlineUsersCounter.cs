using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ModernizationDemo.SignalR.Hubs
{
    public class OnlineUsersCounter : PersistentConnection
    {
        private static int counter = 0;

        protected override async Task OnConnected(IRequest request, string connectionId)
        {
            var newCount = Interlocked.Increment(ref counter);
            await Connection.Broadcast(newCount);

            await base.OnConnected(request, connectionId);
        }

        protected override async Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            var newCount = Interlocked.Decrement(ref counter);
            await Connection.Broadcast(newCount);

            await base.OnDisconnected(request, connectionId, stopCalled);
        }
    }
}