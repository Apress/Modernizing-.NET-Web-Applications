using Microsoft.Owin;
using ModernizationDemo.SignalR.Hubs;
using ModernizationDemo.SignalR.Security;
using Owin;

[assembly: OwinStartup(typeof(ModernizationDemo.SignalR.Startup))]

namespace ModernizationDemo.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<ApiKeyAuthenticationMiddleware>();
            app.MapSignalR();
            app.MapSignalR<OnlineUsersCounter>("/users");
        }
    }
}
