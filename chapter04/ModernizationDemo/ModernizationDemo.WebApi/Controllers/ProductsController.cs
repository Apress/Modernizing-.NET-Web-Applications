using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using ModernizationDemo.WebApi.OpenApi;
using Swashbuckle.Swagger.Annotations;

namespace ModernizationDemo.WebApi.Controllers
{
    [Authorize]
    public class ProductsController : ApiController
    {
        private readonly ProductService service = new ProductService();

        public List<ProductModel> GetProducts()
        {
            return service.GetProducts();
        }

        public ProductModel GetProduct(int id)
        {
            return service.GetProduct(id);
        }

        [HttpPost]
        public int AddProduct([FromBody] ProductModel product)
        {
            return service.AddProduct(product);
        }

        [HttpPut]
        public void UpdateProduct([FromBody] ProductModel product)
        {
            service.UpdateProduct(product);
        }

        [HttpDelete]
        public void RemoveProduct(int id)
        {
            service.RemoveProduct(id);
        }

        [HttpPost]
        [Route("api/Products/{id}/image")]
        [SwaggerOperationFilter(typeof(AcceptFileOperationFilter))]
        public async Task<string> UploadProductImage(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var storagePath = HostingEnvironment.MapPath("~/images");
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            var provider = new MultipartFormDataStreamProvider(storagePath);
            await Request.Content.ReadAsMultipartAsync(provider);
            var file = provider.FileData.Single();
            
            // generate a unique name for the file
            var uniqueName = $"product{id}.png";
            
            // save the file
            var targetPath = Path.Combine(storagePath, uniqueName);
            File.Copy(file.LocalFileName, targetPath, true);
            File.Delete(file.LocalFileName);

            // return the URL
            return $"images/{uniqueName}";
        }
    }
}
