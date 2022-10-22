using System;
using Application.Actors;
using Application.Managers;
using Application.Repositories;
using Dapr.Actors;
using Dapr.Actors.Client;
using Domain.Models;
using Infrastructure.Actors;

namespace Infrastructure.Managers;

/// <summary>
/// Implements a contract for managing orders.
/// </summary>
public class OrderManager : IOrderManager
{
  /// <inheritdoc/>
  public async Task CreateOrderAsync(Order order)
  {
    var actorId = new ActorId(order.OrderId.ToString());
    var orderActor = ActorProxy.Create<IOrderActor>(actorId, OrderActor.ActorType);
    await orderActor.CreateOrderAsync(order);
  }

  /// <inheritdoc/>
  public async Task CheckoutOrderAsync(Guid orderId)
  {
    var actorId = new ActorId(orderId.ToString());
    var orderActor = ActorProxy.Create<IOrderActor>(actorId, OrderActor.ActorType);
    await orderActor.CheckoutOrderAsync(orderId);
  }
}