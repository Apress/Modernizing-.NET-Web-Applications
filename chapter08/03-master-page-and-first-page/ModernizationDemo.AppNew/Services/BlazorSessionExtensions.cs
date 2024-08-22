using System.Web.SessionState;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.SystemWebAdapters;
using Microsoft.AspNetCore.SystemWebAdapters.Features;
using Microsoft.AspNetCore.SystemWebAdapters.SessionState;

namespace ModernizationDemo.AppNew.Services
{
#pragma warning disable SYSWEB1001
    public static class BlazorSessionExtensions
    {
        public static void LoadSessionForBlazorServer(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                if (context.GetEndpoint()?.Metadata.GetMetadata<RootComponentMetadata>() is not null
                    && string.Equals(context.Request.Method, "CONNECT", StringComparison.OrdinalIgnoreCase))
                {
                    var manager = context.RequestServices.GetRequiredService<ISessionManager>();
                    var session = await manager.CreateAsync(context, 
                        new SessionAttribute() { SessionBehavior = SessionStateBehavior.Required });
                    context.Features.Set<ISessionStateFeature>(new BlazorSessionStateFeature(session));
                    session.Dispose();
                }

                await next();
            });
        }

        public static async Task<SessionLock> AcquireSessionLock(this HttpContext context)
        {
            var sessionState = context.Features.Get<ISessionStateFeature>();
            if (sessionState is null)
            {
                throw new InvalidOperationException("Session state is not available.");
            }

            var manager = context.RequestServices.GetRequiredService<ISessionManager>();
            var session = await manager.CreateAsync(context,
                new SessionAttribute() { SessionBehavior = SessionStateBehavior.Required });
            context.Features.Set<ISessionStateFeature>(new BlazorSessionStateFeature(session));

            return new SessionLock(session);
        }
    }

    public class BlazorSessionStateFeature(ISessionState session) : ISessionStateFeature
    {
        public SessionStateBehavior Behavior { get; set; } = SessionStateBehavior.Required;
        public bool IsPreLoad => true;
        public HttpSessionState? Session => new HttpSessionState(session);
        public ISessionState? State { get; set; } = session;
    }

    public class SessionLock(ISessionState session) : IDisposable
    {
        public async Task CommitAsync(CancellationToken ct = default)
        {
            await session.CommitAsync(ct);
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
