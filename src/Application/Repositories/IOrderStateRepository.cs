using System;
using Domain.Models;

namespace Application.Repositories;

/// <summary>
/// Defines a contract for interacting with order state resources.
/// </summary>
public interface IOrderStateRepository
{
  /// <summary>
  /// Persists an order to state.
  /// </summary>
  /// <param name="order">The details of the order.</param>
  Task SaveOrderAsync(Order order);

  /// <summary>
  /// Attemps to find the order. If nothing is found, null will be returned.
  /// </summary>
  /// <param name="orderId">The order identifier.</param>
  /// <returns>The order details. Null if not found.</returns>
  Task<Order?> GetOrderAsync(Guid orderId);
}