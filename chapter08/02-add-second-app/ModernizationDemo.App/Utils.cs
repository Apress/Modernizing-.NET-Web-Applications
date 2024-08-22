using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModernizationDemo.BackendClient;

#if NET
using Microsoft.AspNetCore.SystemWebAdapters;
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