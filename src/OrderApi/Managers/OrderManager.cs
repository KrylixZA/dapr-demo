using System;
using OrderApi.Models;
using OrderApi.Repositories;

namespace OrderApi.Managers
{
    /// <summary>
    /// Implements a contract for managing orders.
    /// </summary>
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Instantiates a new instace of the order manager class.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <inheritdoc/>
        public async Task CreateOrderAsync(Order order)
        {
            await _orderRepository.CreateOrderAsync(order);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Guid>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        /// <inheritdoc/>
        public async Task<Order> GetOrderDetailsAsync(Guid orderId)
        {
            return await _orderRepository.GetOrderDetailsAsync(orderId);
        }
    }
}