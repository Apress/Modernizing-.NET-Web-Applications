using System.Web;
using Microsoft.AspNetCore.SignalR;

namespace ModernizationDemo.AppNew.Migration
{
    public class CacheInvalidationHub(IConfiguration configuration, ILogger<CacheInvalidationHub> logger) : Hub
    {
        public override Task OnConnectedAsync()
        {
            var apiKey = Context.GetHttpContext()!.Request.Headers["X-API-Key"];
            if (!string.Equals(apiKey, configuration["RemoteAppApiKey"], StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException();
            }
            logger.LogDebug("Client connected.");
            return base.OnConnectedAsync();
        }

        public async Task OnCacheEntryInvalidated(string cacheKey)
        {
            logger.LogInformation($"Invalidated cache entry {cacheKey}");
            HttpRuntime.Cache.Remove(cacheKey);

            await Clients.Others.SendAsync("OnCacheEntryInvalidated", cacheKey);
        }

        public static void NotifyCacheEntryInvalidated(IHubContext<CacheInvalidationHub> hubContext, string cacheKey)
        {
            Task.Run(async () =>
            {
                await hubContext.Clients.All.SendAsync("OnCacheEntryInvalidated", cacheKey);
            });
        }
    }
}
