using Application.Repositories;
using Dapr.Client;
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
  public async Task SaveOrderAsync(Order order)
  {
    await _daprClient.SaveStateAsync(
      DaprComponents.OrderStateStore,
      order.OrderId.ToString(),
      order);
  }

  /// <inheritdoc/>
  public async Task<Order?> GetOrderAsync(Guid orderId)
  {
    return await _daprClient.GetStateAsync<Order?>(
      DaprComponents.OrderStateStore,
      orderId.ToString());
  }
}