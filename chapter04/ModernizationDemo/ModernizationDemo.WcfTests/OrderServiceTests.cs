using System.Collections.Generic;
using System.Linq;
using CheckTestOutput;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.WcfTests.Orders;

namespace ModernizationDemo.WcfTests
{
    [TestClass]
    public class OrderServiceTests
    {
        private readonly OutputChecker check = new OutputChecker("testoutputs");

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfHttp)]
        [DataRow(TestEnvironment.WcfHttps)]
        [DataRow(TestEnvironment.WcfTcp)]
        [DataRow(TestEnvironment.WcfCoreHttp)]
        [DataRow(TestEnvironment.WcfCoreHttps)]
        [DataRow(TestEnvironment.WcfCoreTcp)]
        public void GetOrders(TestEnvironment environment)
        {
            var orders = GetClient(environment).GetOrders()
                .Where(o => o.Id < 1000) // ignore orders created in tests
                .ToArray();
            check.CheckJsonObject(orders);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfHttp)]
        [DataRow(TestEnvironment.WcfHttps)]
        [DataRow(TestEnvironment.WcfTcp)]
        [DataRow(TestEnvironment.WcfCoreHttp)]
        [DataRow(TestEnvironment.WcfCoreHttps)]
        [DataRow(TestEnvironment.WcfCoreTcp)]
        public void GetOrder(TestEnvironment environment)
        {
            var order = GetClient(environment).GetOrder(1);
            check.CheckJsonObject(order);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfHttp)]
        [DataRow(TestEnvironment.WcfHttps)]
        [DataRow(TestEnvironment.WcfTcp)]
        [DataRow(TestEnvironment.WcfCoreHttp)]
        [DataRow(TestEnvironment.WcfCoreHttps)]
        [DataRow(TestEnvironment.WcfCoreTcp)]
        public void GetOrder_NotExists(TestEnvironment environment)
        {
            Utils.AssertException(environment, "The item was not found.", () => GetClient(environment).GetOrder(5000));
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfHttp)]
        [DataRow(TestEnvironment.WcfHttps)]
        [DataRow(TestEnvironment.WcfTcp)]
        [DataRow(TestEnvironment.WcfCoreHttp)]
        [DataRow(TestEnvironment.WcfCoreHttps)]
        [DataRow(TestEnvironment.WcfCoreTcp)]
        public void CalculateOrder(TestEnvironment environment)
        {
            var order = new OrderCreateModel()
            {
                OrderItems = new List<OrderCreateItemModel>()
                {
                    new OrderCreateItemModel()
                    {
                        ProductId = 1,
                        Quantity = 2
                    },
                    new OrderCreateItemModel()
                    {
                        ProductId = 3,
                        Quantity = 45
                    }
                }
            };

            var totalPrice = GetClient(environment).CalculateTotalPrice(order);
            Assert.AreEqual(184499.25m, totalPrice);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfHttp)]
        [DataRow(TestEnvironment.WcfHttps)]
        [DataRow(TestEnvironment.WcfTcp)]
        [DataRow(TestEnvironment.WcfCoreHttp)]
        [DataRow(TestEnvironment.WcfCoreHttps)]
        [DataRow(TestEnvironment.WcfCoreTcp)]
        public void CalculateOrder_ProductNotOnSale(TestEnvironment environment)
        {
            var order = new OrderCreateModel()
            {
                OrderItems = new List<OrderCreateItemModel>()
                {
                    new OrderCreateItemModel()
                    {
                        ProductId = 2,
                        Quantity = 1
                    }
                }
            };

            Utils.AssertException(environment, "Order item product not found or not on sale any more!",
                () => GetClient(environment).CalculateTotalPrice(order));
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfHttp)]
        [DataRow(TestEnvironment.WcfHttps)]
        [DataRow(TestEnvironment.WcfTcp)]
        [DataRow(TestEnvironment.WcfCoreHttp)]
        [DataRow(TestEnvironment.WcfCoreHttps)]
        [DataRow(TestEnvironment.WcfCoreTcp)]
        public void CreateAndCompleteOrder(TestEnvironment environment)
        {
            var client = GetClient(environment);

            var create1 = new OrderCreateModel()
            {
                OrderItems = new List<OrderCreateItemModel>()
                {
                    new OrderCreateItemModel()
                    {
                        ProductId = 1,
                        Quantity = 2
                    },
                    new OrderCreateItemModel()
                    {
                        ProductId = 3,
                        Quantity = 45
                    }
                }
            };
            var orderId1 = client.AddOrder(create1);
            var order1 = client.GetOrder(orderId1);
            Assert.AreEqual(OrderStatus.Pending, order1.Status);
            Assert.AreEqual(184499.25m, order1.TotalPrice);
            Assert.AreEqual(2, order1.OrderItems.Count);
            Assert.IsNull(order1.CompletedAt);
            Assert.IsNull(order1.CanceledAt);
            Assert.IsTrue(order1.Id >= 1000);

            client.CompleteOrder(orderId1);

            var order1Completed = client.GetOrder(orderId1);
            Assert.AreEqual(OrderStatus.Completed, order1Completed.Status); 
            Assert.AreEqual(184499.25m, order1.TotalPrice);
            Assert.AreEqual(2, order1.OrderItems.Count);
            Assert.IsNotNull(order1Completed.CompletedAt);
            Assert.IsNull(order1Completed.CanceledAt);
        }

        [DataTestMethod]
        [DataRow(TestEnvironment.WcfHttp)]
        [DataRow(TestEnvironment.WcfHttps)]
        [DataRow(TestEnvironment.WcfTcp)]
        [DataRow(TestEnvironment.WcfCoreHttp)]
        [DataRow(TestEnvironment.WcfCoreHttps)]
        [DataRow(TestEnvironment.WcfCoreTcp)]
        public void CreateAndCancelOrder(TestEnvironment environment)
        {
            var client = GetClient(environment);

            var create1 = new OrderCreateModel()
            {
                OrderItems = new List<OrderCreateItemModel>()
                {
                    new OrderCreateItemModel()
                    {
                        ProductId = 1,
                        Quantity = 2
                    },
                    new OrderCreateItemModel()
                    {
                        ProductId = 3,
                        Quantity = 45
                    }
                }
            };
            var orderId1 = client.AddOrder(create1);
            var order1 = client.GetOrder(orderId1);
            Assert.AreEqual(OrderStatus.Pending, order1.Status);
            Assert.AreEqual(184499.25m, order1.TotalPrice);
            Assert.AreEqual(2, order1.OrderItems.Count);
            Assert.IsNull(order1.CompletedAt);
            Assert.IsNull(order1.CanceledAt);
            Assert.IsTrue(order1.Id >= 1000);

            client.CancelOrder(orderId1);

            var order1Completed = client.GetOrder(orderId1);
            Assert.AreEqual(OrderStatus.Canceled, order1Completed.Status);
            Assert.AreEqual(184499.25m, order1.TotalPrice);
            Assert.AreEqual(2, order1.OrderItems.Count);
            Assert.IsNull(order1Completed.CompletedAt);
            Assert.IsNotNull(order1Completed.CanceledAt);
        }

        private IOrderService GetClient(TestEnvironment environment)
        {
            switch (environment)
            {
                case TestEnvironment.WcfHttp:
                    return new OrderServiceClient("BasicHttpBinding_IOrderService", "http://localhost:50999/OrderService.svc");

                case TestEnvironment.WcfHttps:
                    return new OrderServiceClient("BasicHttpBinding_IOrderService1", "https://localhost:44326/OrderService.svc");

                case TestEnvironment.WcfTcp:
                    return new OrderServiceClient("NetTcpBinding_IOrderService", "net.tcp://localhost:9000/OrderService.svc");

                case TestEnvironment.WcfCoreHttp:
                    return new OrderServiceClient("BasicHttpBinding_IOrderService", "http://localhost:5217/OrderService.svc");

                case TestEnvironment.WcfCoreHttps:
                    return new OrderServiceClient("BasicHttpBinding_IOrderService1", "https://localhost:7226/OrderService.svc");

                case TestEnvironment.WcfCoreTcp:
                    return new OrderServiceClient("NetTcpBinding_IOrderService", "net.tcp://localhost:9001/OrderService.svc");

                default:
                    throw new System.NotSupportedException();
            }
        }
    }
}
