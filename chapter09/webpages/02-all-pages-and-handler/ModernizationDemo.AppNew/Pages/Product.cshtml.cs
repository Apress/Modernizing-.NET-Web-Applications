using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernizationDemo.App;
using ModernizationDemo.AppNew.Model;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.AppNew.Pages
{
    public class ProductModel(ApiClient apiClient) : BasePageModel
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }

        public BackendClient.ProductModel Product { get; set; }

        public string ProductPrice { get; set; }

        public override async Task OnGetAsync()
        {
            await base.OnGetAsync();

            Product = await apiClient.GetProductAsync(Id);
            ProductPrice = Utils.GetProductPriceWithCaching(Id, Currency);
        }
    }
}
