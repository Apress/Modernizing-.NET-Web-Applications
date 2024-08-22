using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.WebApiCore.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService service = new ProductService();

        [HttpGet]
        public List<ProductModel> GetProducts()
        {
            return service.GetProducts();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ProductModel GetProduct(int id)
        {
            return service.GetProduct(id);
        }

        [HttpPost]
        public int AddProduct(ProductModel product)
        {
            return service.AddProduct(product);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public void UpdateProduct(ProductModel product)
        {
            service.UpdateProduct(product);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(204)]
        public void RemoveProduct(int id)
        {
            service.RemoveProduct(id);
        }

        [HttpPost]
        [Route("{id}/image")]
        public async Task<string> UploadProductImage(int id, IFormFile file, IWebHostEnvironment webHostEnvironment)
        {
            var storagePath = Path.Combine(webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            // auto-generate unique name
            var uniqueName = $"product{id}.png";

            // save the file
            var targetPath = Path.Combine(storagePath, uniqueName);
            await using var fs = System.IO.File.OpenWrite(targetPath);
            await file.CopyToAsync(fs);

            // return the URL
            return $"images/{uniqueName}";
        }
    }
}
