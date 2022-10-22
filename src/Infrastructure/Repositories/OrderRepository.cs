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
/// Implements a contract for interacting with order state.
/// </summary>
public class OrderRepository : IOrderRepository
{
  private readonly DaprClient _daprClient;

  /// <summary>
  /// Instantiates a new instance of the order repository class.
  /// </summary>
  public OrderRepository()
  {
    _daprClient = new DaprClientBuilder().Build();
  }

  /// <inheritdoc/>
  public async Task CreateOrderAsync(Order order)
  {
    Console.WriteLine($"Writing to order store for order: {order.OrderId}");
    await _daprClient.SaveStateAsync(DaprComponents.OrderStateStore, order.OrderId.ToString(), order);

    Console.WriteLine($"Creating actor for order: {order.OrderId}");
    var actorType = "MyActor";
    var actorId = new ActorId("1");
    var proxy = ActorProxy.Create<IOrderActor>(actorId, actorType);
    await proxy.CreateOrderAsync(order);
    Console.WriteLine($"Order actor created for order: {order.OrderId}");
  }
}