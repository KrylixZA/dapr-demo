using System;
using Dapr.Actors;
using Domain.Models;

namespace Application.Actors;

/// <summary>
/// Defines a contract for representing an order 
/// </summary>
public interface IOrderActor : IActor
{
  /// <summary>
  /// Creates an actor that represents the order.
  /// </summary>
  /// <param name="newOrder">The details of the order.</param>
  Task CreateOrderAsync(Order newOrder);

  /// <summary>
  /// Begins the checkout process for an order.
  /// This will update the order state and the updated date time field.
  /// </summary>
  /// <param name="orderId">The order unique identifier.</param>
  Task CheckoutOrderAsync(Guid orderId);
}