using System;
using OrderApi.Models;

namespace OrderApi.Repositories
{
    /// <summary>
    /// Defines a contract for interacting with order state.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Persists an order to state.
        /// </summary>
        /// <param name="order">The details of the order.</param>
        Task CreateOrderAsync(Order order);

        /// <summary>
        /// Gets a list of all orders in the state.
        /// </summary>
        /// <returns>The list of orders.</returns>
        Task<IEnumerable<Guid>> GetOrdersAsync();

        /// <summary>
        /// Returns the details of an order by a given identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>The details of the order.</returns>
        Task<Order> GetOrderDetailsAsync(Guid orderId);
    }
}