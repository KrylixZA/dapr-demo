using Application.Repositories;
using Dapr.Client;
using Domain.Constants;
using Domain.Models;

namespace Infrastructure.Repositories;

/// <summary>
/// Implements a contract for interacting with order pub/sub resources.
/// </summary>
public class OrderPubSubRepository : IOrderPubSubRepository
{
  private readonly DaprClient _daprClient;

  /// <summary>
  /// Instantiates a new instance of the order pub/sub repository class.
  /// </summary>
  public OrderPubSubRepository()
  {
    _daprClient = new DaprClientBuilder().Build();
  }

  /// <inheritdoc/>
  public async Task PublishOrderForCheckoutAsync(Order order)
  {
    await _daprClient.PublishEventAsync<Order>(
      DaprComponents.PubSub,
      "checkout",
      order);
  }
}

