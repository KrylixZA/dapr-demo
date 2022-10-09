using System;
using OrderApi.Models;

namespace OrderApi.Managers
{
    /// <summary>
    /// Defines a contract for managing orders.
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Attempts to persist the order to order store.
        /// </summary>
        /// <param name="order">The order details.</param>
        Task CreateOrderAsync(Order order);

        /// <summary>
        /// Gets a list of orders.
        /// </summary>
        /// <returns>The order list.</returns>
        Task<IEnumerable<Guid>> GetOrdersAsync();

        /// <summary>
        /// Gets the details of a specific order.
        /// </summary>
        /// <param name="orderId">The order unique identifier.</param>
        /// <returns>The order details.</returns>
        Task<Order> GetOrderDetailsAsync(Guid orderId);
    }
}