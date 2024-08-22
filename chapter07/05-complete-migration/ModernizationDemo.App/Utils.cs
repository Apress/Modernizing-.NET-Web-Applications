using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App
{
    public class Utils(IMemoryCache memoryCache, ApiClient apiClient)
    {
        public async Task<string> GetProductPriceWithCaching(Guid productId, string selectedCurrency)
        {
            var cacheKey = $"ProductPrice_{selectedCurrency}_{productId}";
            if (!memoryCache.TryGetValue(cacheKey, out var productPrice))
            {
                double? price = null;
                try
                {
                    price = await apiClient.GetProductPriceAsync(productId, selectedCurrency);
                }
                catch (ApiException ex) when (ex.StatusCode == 404)
                {
                }
                productPrice = price != null ? $"{price} {selectedCurrency}" : "Price unavailable";
                memoryCache.Set(cacheKey, productPrice);
            }

            return (string)productPrice;
        }

        public void ResetProductPriceWithCache(Guid productId, string selectedCurrency)
        {
            var cacheKey = $"ProductPrice_{selectedCurrency}_{productId}";
            memoryCache.Remove(cacheKey);
        }
    }
}