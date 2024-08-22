using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.SignalR.Client;

namespace ModernizationDemo.App.Migration
{
    public class CacheInvalidationClient
    {
        private static HubConnection client;
        private static CancellationTokenSource cts = new CancellationTokenSource();

        public static void StartWorker()
        {
            client = new HubConnectionBuilder()
                .WithUrl(new Uri(new Uri(ConfigurationManager.AppSettings["RemoteAppUrl"]), "_migration/cache-invalidate"), options =>
                {
                    options.Headers["X-API-Key"] = ConfigurationManager.AppSettings["RemoteAppApiKey"];
                })
                .WithAutomaticReconnect(new InfiniteRetryPolicy())
                .Build();

            Task.Run(async () =>
            {
                client.On<string>("OnCacheEntryInvalidated", cacheKey =>
                {
                    Debug.WriteLine($"Cache invalidation: Invalidating cache entry {cacheKey}");
                    HttpRuntime.Cache.Remove(cacheKey);
                });

                // connect and retry every 5 seconds on error
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        await client.StartAsync(cts.Token);
                        Debug.WriteLine("Cache invalidation: Connected");

                        // wait until the process ends
                        await Task.Delay(-1, cts.Token);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Cache invalidation: Connect failed, will retry in 5 seconds...");
                        await Task.Delay(5000, cts.Token);
                    }
                }
            });
        }

        public static void StopWorker()
        {
            cts.Cancel();
        }

        public static void NotifyCacheEntryInvalidated(string cacheKey)
        {
            Task.Run(async () =>
            {
                try
                {
                    await client.SendAsync("OnCacheEntryInvalidated", cacheKey);
                    Debug.WriteLine($"Cache invalidation: Notified cache entry {cacheKey}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Cache invalidation: Send failure " + ex);
                }
            });
        }
    }

    public class InfiniteRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            return TimeSpan.FromSeconds(5);
        }
    }
}