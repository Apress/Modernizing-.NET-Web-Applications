using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App.ViewModels
{
    public class ProductDetailViewModel(ApiClient apiClient) : SiteViewModel
    {
        [FromRoute("Id")]
        public Guid Id { get; set; }

        public ProductModel Product { get; set; }

        public string Price { get; set; }

        public int Quantity { get; set; }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Product = await apiClient.GetProductAsync(Id);
                Price = Utils.GetProductPriceWithCaching(Id, SelectedCurrency);
            }
            await base.PreRender();
        }

    }
}

