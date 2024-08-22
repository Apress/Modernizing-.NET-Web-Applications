using Microsoft.AspNetCore.Mvc;
using ModernizationDemo.AppNew.Model;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.AppNew.Pages
{
    public class IndexModel(ApiClient apiClient, Utils utils) : BasePageModel
    {
        [FromQuery(Name = "page")]
        public int PageIndex { get; set; }

        public ProductModelPagedResponse Products { get; set; }

        public Dictionary<Guid, string> ProductPrices { get; set; }

        public PagerModel PagerModel { get; set; }

        public override async Task OnGetAsync()
        {
            await base.OnGetAsync();
            
            const int pageSize = 12;

            Products = await apiClient.GetProductsAsync(PageIndex * pageSize, pageSize);
            ProductPrices = new Dictionary<Guid, string>();
            foreach (var product in Products.Results)
            {
                var price = await utils.GetProductPriceWithCaching(product.Id, Currency);
                ProductPrices[product.Id] = price;
            }

            PagerModel = new PagerModel() { PageIndex = PageIndex, PagesCount = (int)Math.Ceiling((double)Products.TotalRecordCount / pageSize) };
        }
    }
}
