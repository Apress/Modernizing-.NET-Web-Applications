using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModernizationDemo.BackendClient;
using Microsoft.Extensions.Caching.Memory;

namespace ModernizationDemo.AppNew
{
    public class Utils(ApiClient apiClient, IMemoryCache memoryCache)
    {
        public async Task<string> GetProductPriceWithCaching(Guid productId, string selectedCurrency)
        {
            var cacheKey = $"ProductPrice_{selectedCurrency}_{productId}";
            var productPrice = memoryCache.Get(cacheKey) as string;
            if (productPrice == null)
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
            return productPrice;
        }

        public void ResetProductPriceWithCache(Guid productId, string selectedCurrency)
        {
            var cacheKey = $"ProductPrice_{selectedCurrency}_{productId}";
            memoryCache.Remove(cacheKey);
        }
    }
}