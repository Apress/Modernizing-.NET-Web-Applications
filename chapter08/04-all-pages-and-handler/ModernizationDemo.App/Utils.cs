using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModernizationDemo.BackendClient;

#if NET
using Microsoft.AspNetCore.SystemWebAdapters;
using Microsoft.AspNetCore.SignalR;
using ModernizationDemo.AppNew.Migration;
#else 
using ModernizationDemo.App.Migration;
#endif

namespace ModernizationDemo.App
{
    public class Utils
    {
        public static string GetProductPriceWithCaching(Guid productId, string selectedCurrency)
        {
            var cacheKey = $"ProductPrice_{selectedCurrency}_{productId}";
            var productPrice = HttpRuntime.Cache[cacheKey] as string;
            if (productPrice == null)
            {
                double? price = null;
                try
                {
                    price = GetApiClient().GetProductPrice(productId, selectedCurrency);
                }
                catch (ApiException ex) when (ex.StatusCode == 404)
                {
                }
                productPrice = price != null ? $"{price} {selectedCurrency}" : "Price unavailable";
                HttpRuntime.Cache[cacheKey] = productPrice;
            }
            return productPrice;
        }

        public static void ResetProductPriceWithCache(Guid productId, string selectedCurrency)
        {
            var cacheKey = $"ProductPrice_{selectedCurrency}_{productId}";
            HttpRuntime.Cache.Remove(cacheKey);
            NotifyCacheEntryInvalidated(cacheKey);
        }

        public static void NotifyCacheEntryInvalidated(string cacheKey)
        {
#if !NET
            CacheInvalidationClient.NotifyCacheEntryInvalidated(cacheKey);
#else
            var hubContext = System.Web.HttpContext.Current!.AsAspNetCore()
                .RequestServices.GetRequiredService<IHubContext<CacheInvalidationHub>>();
            CacheInvalidationHub.NotifyCacheEntryInvalidated(hubContext, cacheKey);
#endif 
        }

        private static ApiClient GetApiClient()
        {
#if !NET
            return Global.GetApiClient();
#else
            return System.Web.HttpContext.Current!.AsAspNetCore().RequestServices.GetRequiredService<ApiClient>();
#endif
        }
    }
}