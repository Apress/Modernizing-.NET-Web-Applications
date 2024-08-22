using System.Configuration;
using System.Linq;
using CheckTestOutput;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModernizationDemo.SoapGrpc;
using ModernizationDemo.SoapTests.Services;

namespace ModernizationDemo.SoapTests
{
    [TestClass]
    public class ProductServiceTests
    {
        private readonly OutputChecker check = new OutputChecker("testoutputs");

        [DataTestMethod]
        [DataRow(TestEnvironment.Soap)]
        [DataRow(TestEnvironment.SoapCore)]
        [DataRow(TestEnvironment.Grpc)]
        public void GetProducts(TestEnvironment environment)
        {
            var products = GetClient(environment).GetProducts()
                .Where(p => p.Id < 100)
                .ToArray();
            check.CheckJsonObject(products);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.Soap)]
        [DataRow(TestEnvironment.SoapCore)]
        [DataRow(TestEnvironment.Grpc)]
        public void GetProduct(TestEnvironment environment)
        {
            var product = GetClient(environment).GetProduct(10);
            check.CheckJsonObject(product);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.Soap)]
        [DataRow(TestEnvironment.SoapCore)]
        [DataRow(TestEnvironment.Grpc)]
        public void GetProduct_NotExists(TestEnvironment environment)
        {
            Utils.AssertException(environment, "The item was not found.", () => GetClient(environment).GetProduct(5000));
        }


        [DataTestMethod]
        [DataRow(TestEnvironment.Soap)]
        [DataRow(TestEnvironment.SoapCore)]
        [DataRow(TestEnvironment.Grpc)]
        public void AddUpdateRemoveProduct(TestEnvironment environment)
        {
            var client = GetClient(environment);

            var product1 = new ProductsClient.ProductModel()
            {
                Title = "Test product",
                Description = "Test description",
                IsOnSale = true,
                Unit = "package",
                UnitPrice = 10.5m
            };
            var productId = client.AddProduct(product1);
            product1.Id = productId;

            var product2 = client.GetProduct(productId);
            Assert.AreEqual(productId, product2.Id);
            Assert.AreEqual(product1.Title, product2.Title);
            Assert.AreEqual(product1.Description, product2.Description);
            Assert.AreEqual(product1.IsOnSale, product2.IsOnSale);
            Assert.AreEqual(product1.Unit, product2.Unit);
            Assert.AreEqual(product1.UnitPrice, product2.UnitPrice);
            Assert.AreEqual(product1.ImageUrl, product2.ImageUrl);

            product1.Description = "new description";
            product1.UnitPrice = 20.5m;
            client.UpdateProduct(product1);

            product2 = client.GetProduct(productId);
            Assert.AreEqual(productId, product2.Id);
            Assert.AreEqual(product1.Title, product2.Title);
            Assert.AreEqual(product1.Description, product2.Description);
            Assert.AreEqual(product1.IsOnSale, product2.IsOnSale);
            Assert.AreEqual(product1.Unit, product2.Unit);
            Assert.AreEqual(product1.UnitPrice, product2.UnitPrice);
            Assert.AreEqual(product1.ImageUrl, product2.ImageUrl);

            client.RemoveProduct(productId);
            product2 = client.GetProduct(productId);
            Assert.IsFalse(product2.IsOnSale);
        }

        private IProductsClient GetClient(TestEnvironment environment)
        {
            switch (environment)
            {
                case TestEnvironment.Soap:
                    return new SoapProductsClient("http://localhost:50933/Products.asmx");

                case TestEnvironment.SoapCore:
                    return new SoapProductsClient("http://localhost:5067/Products.asmx");

                case TestEnvironment.Grpc:
                    return new GrpcProductsClient("https://localhost:7090/");

                default:
                    throw new System.NotSupportedException();
            }
        }
    }
}
