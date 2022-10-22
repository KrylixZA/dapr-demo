using System;
using System.Xml.Linq;
using Application.Actors;
using Application.Repositories;
using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using Domain;
using Domain.Constants;
using Domain.Models;

namespace Infrastructure.Repositories;

/// <summary>
/// Implements a contract for interacting with order state resources.
/// </summary>
public class OrderStateRepository : IOrderStateRepository
{
  private readonly DaprClient _daprClient;

  /// <summary>
  /// Instantiates a new instance of the order state repository class.
  /// </summary>
  public OrderStateRepository()
  {
    _daprClient = new DaprClientBuilder().Build();
  }

  /// <inheritdoc/>
  public async Task CreateOrderAsync(Order newOrder)
  {
    await _daprClient.SaveStateAsync(
      DaprComponents.OrderStateStore,
      newOrder.OrderId.ToString(),
      newOrder);
  }

  /// <inheritdoc/>
  public async Task<Order?> GetOrderAsync(Guid orderId)
  {
    return await _daprClient.GetStateAsync<Order?>(
      DaprComponents.OrderStateStore,
      orderId.ToString());
  }

  /// <inheritdoc/>
  public async Task CheckoutOrderAsync(Order updatedOrder)
  {
    await _daprClient.SaveStateAsync(
      DaprComponents.OrderStateStore,
      updatedOrder.OrderId.ToString(),
      updatedOrder);
  }
}