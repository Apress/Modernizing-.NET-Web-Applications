using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using ModernizationDemo.BackendClient;
using DotVVM.Framework.Controls;

namespace ModernizationDemo.App.ViewModels
{
    public class DefaultViewModel(ApiClient apiClient) : SiteViewModel
    {
        public GridViewDataSet<ProductModel> Products { get; set; } = new()
        {
            PagingOptions = { PageSize = 12 }
        };

        public Dictionary<Guid, string> Prices { get; set; }

        public override async Task PreRender()
        {
            if (Products.IsRefreshRequired)
            {
                var response = await apiClient.GetProductsAsync(
                    Products.PagingOptions.PageIndex * Products.PagingOptions.PageSize,
                    Products.PagingOptions.PageSize);
                Products.Items = response.Results.ToList();
                Products.PagingOptions.TotalItemsCount = response.TotalRecordCount;

                Prices = response.Results.ToDictionary(
                    p => p.Id, 
                    p => Utils.GetProductPriceWithCaching(p.Id, SelectedCurrency));
            }
            await base.PreRender();
        }
    }
}

