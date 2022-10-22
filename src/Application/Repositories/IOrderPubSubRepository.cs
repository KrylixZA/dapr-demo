using Domain.Models;

namespace Application.Repositories;

/// <summary>
/// Defines a contract for interacting with order pub/sub resources.
/// </summary>
public interface IOrderPubSubRepository
{
  /// <summary>
  /// Publishes a messages to the "orders" queue with the details of the order to checkout.
  /// </summary>
  /// <param name="order">The order.</param>
  Task PublishOrderForCheckoutAsync(Order order);
}