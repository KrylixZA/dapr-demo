using System;
using Domain.Models;

namespace Application.Repositories;

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
}