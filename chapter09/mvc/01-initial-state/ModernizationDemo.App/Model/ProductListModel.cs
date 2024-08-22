using System;
using System.Collections.Generic;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App.Model
{
    public class ProductListModel
    {
        public ProductModelPagedResponse  Products { get; set; }

        public Dictionary<Guid, string> ProductPrices { get; set; }

        public PagerModel PagerModel { get; set; }
    }
}