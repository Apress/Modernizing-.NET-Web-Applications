using System;
using System.Linq;
using System.ServiceModel.Security;
using CheckTestOutput;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.WcfTests.Products;

namespace ModernizationDemo.WcfTests
{
    [TestClass]
    public class ProductServiceTests
    {
        private readonly OutputChecker check = new OutputChecker("testoutputs");

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfAuthenticated)]
        [DataRow(TestEnvironment.WcfCoreAuthenticated)]
        public void GetProducts(TestEnvironment environment)
        {
            var products = GetClient(environment).GetProducts()
                .Where(p => p.Id < 100)
                .ToArray();
            check.CheckJsonObject(products);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfAuthenticated)]
        [DataRow(TestEnvironment.WcfCoreAuthenticated)]
        public void GetProduct(TestEnvironment environment)
        {
            var product = GetClient(environment).GetProduct(10);
            check.CheckJsonObject(product);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfAuthenticated)]
        [DataRow(TestEnvironment.WcfCoreAuthenticated)]
        public void GetProduct_NotExists(TestEnvironment environment)
        {
            Utils.AssertException(environment, "The item was not found.", () => GetClient(environment).GetProduct(5000));
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfAuthenticated)]
        [DataRow(TestEnvironment.WcfCoreAuthenticated)]
        public void AddUpdateRemoveProduct(TestEnvironment environment)
        {
            var client = GetClient(environment);

            var product1 = new ProductModel()
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

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfAuthenticated)]
        [DataRow(TestEnvironment.WcfCoreAuthenticated)]
        public void GetProduct_Unauthenticated(TestEnvironment environment)
        {
            Assert.ThrowsException<InvalidOperationException>(() => GetClient(environment, null, null).GetProduct(10));
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfAuthenticated)]
        [DataRow(TestEnvironment.WcfCoreAuthenticated)]
        public void GetProduct_InvalidCredentials(TestEnvironment environment)
        {
            Assert.ThrowsException<MessageSecurityException>(() => GetClient(environment, "test", "invalid-password").GetProduct(10));
        }

        private IProductService GetClient(TestEnvironment environment, string username = "test", string password = "test-password")
        {
            ProductServiceClient client;

            switch (environment)
            {
                case TestEnvironment.WcfAuthenticated:
                    client = new ProductServiceClient("WSHttpBinding_IProductService", "https://localhost:44326/ProductService.svc");
                    break;

                case TestEnvironment.WcfCoreAuthenticated:
                    client = new ProductServiceClient("WSHttpBinding_IProductService", "https://localhost:7226/ProductService.svc");
                    break;

                default:
                    throw new System.NotSupportedException();
            }

            if (username != null)
            {
                client.ClientCredentials.UserName.UserName = username;
                client.ClientCredentials.UserName.Password = password;
            }

            return client;
        }
    }
}
