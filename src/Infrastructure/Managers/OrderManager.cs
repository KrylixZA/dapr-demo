using System;
using Application.Managers;
using Application.Repositories;
using Domain.Models;

namespace Infrastructure.Managers;

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
  public Task CheckoutOrderAsync(Guid orderId)
  {
    throw new NotImplementedException();
  }
}