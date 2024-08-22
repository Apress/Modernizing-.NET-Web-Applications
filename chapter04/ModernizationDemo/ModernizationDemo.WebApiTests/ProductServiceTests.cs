using CheckTestOutput;
using ModernizationDemo.WebApi.Client;

namespace ModernizationDemo.WebApiTests
{
    public class ProductServiceTests
    {

        private readonly OutputChecker check = new OutputChecker("testoutputs");

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetProducts(TestEnvironment environment)
        {
            var products = (await GetClient(environment).GetProductsAsync())
                .Where(p => p.Id < 100)
                .ToArray();
            check.CheckJsonObject(products);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetProduct(TestEnvironment environment)
        {
            var product = await GetClient(environment).GetProductAsync(10);
            check.CheckJsonObject(product);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetProduct_NotExists(TestEnvironment environment)
        {
            await Utils.AssertException(environment, "The item was not found.", () => GetClient(environment).GetProductAsync(5000));
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task AddUpdateRemoveProduct(TestEnvironment environment)
        {
            var client = GetClient(environment);

            var product1 = new ProductModel()
            {
                Title = "Test product",
                Description = "Test description",
                IsOnSale = true,
                Unit = "package",
                UnitPrice = 10.5
            };
            var productId = await client.AddProductAsync(product1);
            product1.Id = productId;

            var product2 = await client.GetProductAsync(productId);
            Assert.Equal(productId, product2.Id);
            Assert.Equal(product1.Title, product2.Title);
            Assert.Equal(product1.Description, product2.Description);
            Assert.Equal(product1.IsOnSale, product2.IsOnSale);
            Assert.Equal(product1.Unit, product2.Unit);
            Assert.Equal(product1.UnitPrice, product2.UnitPrice);
            Assert.Equal(product1.ImageUrl, product2.ImageUrl);

            product1.Description = "new description";
            product1.UnitPrice = 20.5;
            await client.UpdateProductAsync(product1);

            product2 = await client.GetProductAsync(productId);
            Assert.Equal(productId, product2.Id);
            Assert.Equal(product1.Title, product2.Title);
            Assert.Equal(product1.Description, product2.Description);
            Assert.Equal(product1.IsOnSale, product2.IsOnSale);
            Assert.Equal(product1.Unit, product2.Unit);
            Assert.Equal(product1.UnitPrice, product2.UnitPrice);
            Assert.Equal(product1.ImageUrl, product2.ImageUrl);

            await client.RemoveProductAsync(productId);
            product2 = await client.GetProductAsync(productId);
            Assert.False(product2.IsOnSale);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetProduct_Unauthenticated(TestEnvironment environment)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(() => GetClient(environment, null).GetProductAsync(10));
            Assert.Equal(401, ex.StatusCode);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetProduct_InvalidCredentials(TestEnvironment environment)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(() => GetClient(environment, "invalid-token").GetProductAsync(10));
            Assert.Equal(401, ex.StatusCode);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task UploadProductImage(TestEnvironment environment)
        {
            var client = GetClient(environment);

            var fileBytes = await File.ReadAllBytesAsync("image.png");
            var file = new FileParameter(new MemoryStream(fileBytes), "image.png");
            var targetUrl = await client.UploadProductImageAsync(15, file);

            var url = client.BaseUrl + targetUrl;
            var downloadedFile = await new HttpClient().GetByteArrayAsync(url);
            Assert.Equal(fileBytes, downloadedFile);
        }

        private ProductsServiceClient GetClient(TestEnvironment environment, string? bearerToken = "test-key-1")
        {
            var httpClient = new HttpClient();
            if (bearerToken is not null)
            {
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", bearerToken);
            }

            return new ProductsServiceClient(httpClient)
            {
                BaseUrl = environment switch
                {
                    TestEnvironment.WebApi => "https://localhost:44398/",
                    TestEnvironment.WebApiCore => "https://localhost:7085/",
                    _ => throw new NotSupportedException()
                }
            };
        }
    }
}
