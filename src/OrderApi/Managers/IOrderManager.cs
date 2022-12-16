using Domain.Models;

namespace OrderApi.Managers;

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
  /// <param name="orderId">The order identifier.</param>
  Task CheckoutOrderAsync(Guid orderId);

  /// <summary>
  /// Completes an order.
  /// </summary>
  /// <param name="orderId">The order identifier.</param>
  Task CompleteOrderAsync(Guid orderId);

  /// <summary>
  /// Deactivates the order actor.
  /// </summary>
  /// <param name="orderId">The order identifier.</param>
  Task DeactivateOrderActorAsync(Guid orderId);
}