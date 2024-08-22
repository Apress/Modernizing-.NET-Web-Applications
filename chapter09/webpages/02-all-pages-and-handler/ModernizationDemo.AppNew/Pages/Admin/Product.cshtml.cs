using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernizationDemo.AppNew.Model;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.AppNew.Pages.Admin
{
    public class ProductModel(ApiClient apiClient) : BasePageModel
    {
        [FromRoute(Name = "id")]
        public Guid? Id { get; set; }

        [BindProperty]
        public ProductDetailModel Product { get; set; }

        public override async Task OnGetAsync()
        {
            await base.OnGetAsync();

            if (Id == null)
            {
                Product = new ProductDetailModel();
            }
            else
            {
                var data = await apiClient.GetProductAsync(Id.Value);
                Product = new ProductDetailModel
                {
                    Name = data.Name,
                    Description = data.Description,
                    ImageUrl = data.ImageUrl
                };
            }
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var data = new ProductCreateEditModel
            {
                Name = Product.Name,
                Description = Product.Description,
                ImageUrl = Product.ImageUrl
            };
            if (Id == null)
            {
                await apiClient.AddProductAsync(data);
            }
            else
            {
                await apiClient.UpdateProductAsync(Id.Value, data);
            }

            return RedirectToPage("Products");
        }
    }
}
