using System;
using Domain.Models;

namespace Application.Managers;

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
  /// Begins the checkout process for order.
  /// </summary>
  /// <remarks>
  /// An order is represented by an actor. T
  /// </remarks>
  /// <param name="orderId"></param>
  /// <returns></returns>
  Task CheckoutOrderAsync(Guid orderId);
}