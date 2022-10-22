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
  /// <param name="order">The details of the order.</param>
  Task CreateOrderAsync(Order order);
}