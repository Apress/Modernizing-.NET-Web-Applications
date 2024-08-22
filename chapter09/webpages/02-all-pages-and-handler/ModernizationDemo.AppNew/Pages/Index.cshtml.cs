using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernizationDemo.App;
using ModernizationDemo.AppNew.Model;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.AppNew.Pages
{
    public class IndexModel(ApiClient apiClient) : BasePageModel
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
            ProductPrices = Products.Results.ToDictionary(p => p.Id, p => Utils.GetProductPriceWithCaching(p.Id, Currency));

            PagerModel = new PagerModel() { PageIndex = PageIndex, PagesCount = (int)Math.Ceiling((double)Products.TotalRecordCount / pageSize) };
        }
    }
}
