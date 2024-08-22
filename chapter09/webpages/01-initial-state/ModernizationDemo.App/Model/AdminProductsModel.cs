using System;
using System.Collections.Generic;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App.Model
{
    public class AdminProductsModel
    {
        public ProductModelPagedResponse  Products { get; set; }

        public Dictionary<Guid, string> ProductPrices { get; set; }

        public PagerModel PagerModel { get; set; }
    }
}