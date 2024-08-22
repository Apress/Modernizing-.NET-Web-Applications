using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using ModernizationDemo.BackendClient;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel.Validation;

namespace ModernizationDemo.App.ViewModels.Admin
{
    public class ProductDetailViewModel(ApiClient apiClient) : ModernizationDemo.App.ViewModels.SiteViewModel
    {

        [FromRoute("Id")]
        public Guid? ProductId { get; set; }

        public bool IsEdit => ProductId != null;

        public ProductCreateEditModel Product { get; set; }

        public GridViewDataSet<ProductPriceModel> Prices { get; set; } = new()
        {
            RowEditOptions =
            {
                PrimaryKeyPropertyName = nameof(ProductPriceModel.CurrencyCode)
            }
        };

        public ProductPriceModel NewPrice { get; set; }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                await LoadProduct();
            }

            if (IsEdit && Prices.IsRefreshRequired)
            {
                await LoadPrices();
            }

            await base.PreRender();
        }

        private async Task LoadProduct()
        {
            if (!IsEdit)
            {
                Product = new ProductCreateEditModel();
            }
            else
            {
                var product = await apiClient.GetProductAsync(ProductId.Value);
                Product = new ProductCreateEditModel
                {
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl
                };
            }
        }

        public async Task InsertOrUpdate()
        {
            if (!IsEdit)
            {
                var id = await apiClient.AddProductAsync(Product);
                Context.RedirectToRouteHybrid("AdminProductDetail", new { Id = id });
            }
            else
            {
                await apiClient.UpdateProductAsync(ProductId.Value, Product);
                Context.RedirectToRouteHybrid("AdminProducts");
            }
        }


        private async Task LoadPrices()
        {
            var result = await apiClient.GetProductPricesAsync(ProductId.Value);

            Prices.Items = result.ToList();
            NewPrice = new ProductPriceModel { CurrencyCode = "USD" };
        }

        public async Task InsertPrice()
        {
            var existingPrices = await apiClient.GetProductPricesAsync(ProductId.Value);
            if (existingPrices.Any(p => p.CurrencyCode == NewPrice.CurrencyCode))
            {
                this.AddModelError(vm => vm.NewPrice.CurrencyCode, "Price for this currency is already set!");
                Context.FailOnInvalidModelState();
            }

            await apiClient.AddOrUpdateProductPriceAsync(ProductId.Value, NewPrice.CurrencyCode, NewPrice.Price);
            Prices.RequestRefresh();
        }

        public async Task UpdatePrice(ProductPriceModel price)
        {
            await apiClient.AddOrUpdateProductPriceAsync(ProductId.Value, price.CurrencyCode, price.Price);
            Prices.RequestRefresh();
            Prices.RowEditOptions.EditRowId = null;
        }

        public async Task DeletePrice(ProductPriceModel price)
        {
            await apiClient.DeleteProductPriceAsync(ProductId.Value, price.CurrencyCode);
            Utils.ResetProductPriceWithCache(ProductId.Value, price.CurrencyCode);
            Prices.RequestRefresh();
        }

    }
}

