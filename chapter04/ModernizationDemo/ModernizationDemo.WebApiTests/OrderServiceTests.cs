using CheckTestOutput;
using ModernizationDemo.WebApi.Client;

namespace ModernizationDemo.WebApiTests
{
    public class OrderServiceTests
    {
        private readonly OutputChecker check = new OutputChecker("testoutputs");

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetOrders(TestEnvironment environment)
        {
            var orders = (await GetClient(environment).GetOrdersAsync())
                .Where(o => o.Id < 1000) // ignore orders created in tests
                .ToArray();
            check.CheckJsonObject(orders);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetOrder(TestEnvironment environment)
        {
            var order = await GetClient(environment).GetOrderAsync(1);
            check.CheckJsonObject(order);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task GetOrder_NotExists(TestEnvironment environment)
        {
            await Utils.AssertException(environment, "The item was not found.", () => GetClient(environment).GetOrderAsync(5000));
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task CalculateOrder(TestEnvironment environment)
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

            var totalPrice = await GetClient(environment).CalculateTotalPriceAsync(order);
            Assert.Equal(184499.25, totalPrice);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task CalculateOrder_ProductNotOnSale(TestEnvironment environment)
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

            await Utils.AssertException(environment, "Order item product not found or not on sale any more!",
                () => GetClient(environment).CalculateTotalPriceAsync(order));
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task CreateAndCompleteOrder(TestEnvironment environment)
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
            var orderId1 = await client.AddOrderAsync(create1);
            var order1 = await client.GetOrderAsync(orderId1);
            Assert.Equal(OrderModelStatus.Pending, order1.Status);
            Assert.Equal(184499.25, order1.TotalPrice);
            Assert.Equal(2, order1.OrderItems.Count);
            Assert.Null(order1.CompletedAt);
            Assert.Null(order1.CanceledAt);
            Assert.True(order1.Id >= 1000);

            await client.CompleteOrderAsync(orderId1);

            var order1Completed = await client.GetOrderAsync(orderId1);
            Assert.Equal(OrderModelStatus.Completed, order1Completed.Status);
            Assert.Equal(184499.25, order1.TotalPrice);
            Assert.Equal(2, order1.OrderItems.Count);
            Assert.NotNull(order1Completed.CompletedAt);
            Assert.Null(order1Completed.CanceledAt);
        }

        [Theory]
        [InlineData(TestEnvironment.WebApi)]
        [InlineData(TestEnvironment.WebApiCore)]
        public async Task CreateAndCancelOrder(TestEnvironment environment)
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
            var orderId1 = await client.AddOrderAsync(create1);
            var order1 = await client.GetOrderAsync(orderId1);
            Assert.Equal(OrderModelStatus.Pending, order1.Status);
            Assert.Equal(184499.25, order1.TotalPrice);
            Assert.Equal(2, order1.OrderItems.Count);
            Assert.Null(order1.CompletedAt);
            Assert.Null(order1.CanceledAt);
            Assert.True(order1.Id >= 1000);

            await client.CancelOrderAsync(orderId1);

            var order1Completed = await client.GetOrderAsync(orderId1);
            Assert.Equal(OrderModelStatus.Canceled, order1Completed.Status);
            Assert.Equal(184499.25, order1.TotalPrice);
            Assert.Equal(2, order1.OrderItems.Count);
            Assert.Null(order1Completed.CompletedAt);
            Assert.NotNull(order1Completed.CanceledAt);
        }

        private OrdersServiceClient GetClient(TestEnvironment environment)
        {
            var httpClient = new HttpClient();
            return new OrdersServiceClient(httpClient)
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