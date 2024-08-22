using Grpc.Core;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.SoapGrpc.Services
{
    public class ProductsImplementation : Products.ProductsBase
    {
        private readonly ProductService service = new ProductService();

        public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
        {
            return new GetProductsResponse()
            {
                Result = { service.GetProducts().Select(MapToDto) }
            };
        }
        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            return new GetProductResponse()
            {
                Result = MapToDto(service.GetProduct(request.Id))
            };
        }

        public override async Task<AddProductResponse> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            return new AddProductResponse()
            {
                Result = service.AddProduct(MapFromDto(request.Model))
            };
        }
        public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            service.UpdateProduct(MapFromDto(request.Model));
            return new UpdateProductResponse();
        }

        public override async Task<RemoveProductResponse> RemoveProduct(RemoveProductRequest request, ServerCallContext context)
        {
            service.RemoveProduct(request.Id);
            return new RemoveProductResponse();
        }


        private static ProductModel MapToDto(BusinessLogic.Models.ProductModel product)
        {
            return new ProductModel()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                IsOnSale = product.IsOnSale,
                Unit = product.Unit,
                UnitPrice = (double)product.UnitPrice
            };
        }

        private static BusinessLogic.Models.ProductModel MapFromDto(ProductModel product)
        {
            return new BusinessLogic.Models.ProductModel()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                IsOnSale = product.IsOnSale,
                Unit = product.Unit,
                UnitPrice = (decimal)product.UnitPrice
            };
        }
    }
}
