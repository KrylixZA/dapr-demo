using System;
using Dapr.Client;
using OrderApi.Models;

namespace OrderApi.Repositories
{
    /// <summary>
    /// Implements a contract for interacting with order state.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly DaprClient _daprClient;

        private const string ORDER_STORE = "orders";

        /// <summary>
        /// Instantiates a new instance of the order repository class.
        /// </summary>
        public OrderRepository()
        {
            _daprClient = new DaprClientBuilder().Build();
        }

        /// <inheritdoc/>
        public async Task CreateOrderAsync(Order order)
        {
            await _daprClient.SaveStateAsync(ORDER_STORE, order.OrderId.ToString(), order);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Guid>> GetOrdersAsync()
        {
            //await _daprClient.GetStateAsync(ORDER_STORE);
            var orders = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            return Task.FromResult<IEnumerable<Guid>>(orders);
        }

        /// <inheritdoc/>
        public Task<Order> GetOrderDetailsAsync(Guid orderId)
        {
            return Task.FromResult(new Order
            {
                OrderId = Guid.NewGuid(),
                Items = new List<Item>
                {
                    new Item
                    {
                        ItemId = Guid.NewGuid(),
                        ItemName = "Peanut butter",
                        ItemDescription = "Black Cat Super Smooth Peanut Butter 500g",
                        Cost = 47.99
,                   },
                    new Item
                    {
                        ItemId = Guid.NewGuid(),
                        ItemName = "Honey",
                        ItemDescription = "Peels Honey 500g",
                        Cost = 79.99
,                   }
                }
            });
        }
    }
}