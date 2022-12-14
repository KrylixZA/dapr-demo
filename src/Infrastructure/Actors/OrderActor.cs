using Application.Actors;
using Application.Repositories;
using Dapr.Actors.Runtime;
using Domain.Constants;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Actors;

/// <summary>
/// Implements a contract for my test actor.
/// </summary>
public class OrderActor : Actor, IOrderActor
{
  private readonly IOrderStateRepository _orderStateRepository;
  private readonly IOrderPubSubRepository _orderPubSubRepository;

  /// <summary>
  /// The actor type.
  /// </summary>
  public static string ActorType => nameof(OrderActor);

  // The constructor must accept ActorHost as a parameter, and can also accept additional
  // parameters that will be retrieved from the dependency injection container
  //
  /// <summary>
  /// Initializes a new instance of MyActor
  /// </summary>
  /// <param name="host">The Dapr.Actors.Runtime.ActorHost that will host this actor instance.</param>
  /// <param name="orderStateRepository">The order state repository.</param>
  /// <param name="orderPubSubRepository">The order pub/sub repository.</param>
  public OrderActor(
    ActorHost host,
    IOrderStateRepository orderStateRepository,
    IOrderPubSubRepository orderPubSubRepository)
      : base(host)
  {
    _orderStateRepository = orderStateRepository;
    _orderPubSubRepository = orderPubSubRepository;
  }

  /// <inheritdoc />
  public async Task CreateOrderAsync(Order order)
  {
    Logger.LogDebug("CreateOrderAsync start. OrderId: {orderId}", order.OrderId);

    var actorState = new OrderActorState
    {
      ActorCreatedDateTimeUtc = DateTime.UtcNow,
      Order = order
    };

    await Task.WhenAll(
      _orderStateRepository.CreateOrderAsync(order),
      _orderPubSubRepository.PublishOrderEvent(order),
      StateManager.SetStateAsync(DaprComponents.OrderActorStateStore, actorState));

    Logger.LogDebug("CreateOrderAsync end. OrderId: {orderId}", order.OrderId);
  }

  /// <inheritdoc />
  public async Task CheckoutOrderAsync(Guid orderId)
  {
    Logger.LogDebug("CheckoutOrderAsync start. OrderId: {orderId}", orderId);

    var actorState = await GetActorStateAsync();
    var order = actorState.Order;

    // Update order details and representative actor state.
    order.OrderUpdatedDateTimeUtc = DateTime.UtcNow;
    order.OrderState = OrderState.CheckOut;
    actorState.UpdateOrder(order);

    await Task.WhenAll(
      _orderStateRepository.CheckoutOrderAsync(order),
      _orderPubSubRepository.PublishOrderEvent(order),
      StateManager.SetStateAsync(DaprComponents.OrderActorStateStore, actorState));

    Logger.LogDebug("CheckoutOrderAsync end. OrderId: {orderId}", orderId);
  }

  /// <inheritdoc />
  public async Task MarkOrderAsCompletedAsync(Guid orderId)
  {
    Logger.LogDebug("MarkOrderAsCompletedAsync start. OrderId: {orderId}", orderId);

    var actorState = await GetActorStateAsync();
    var order = actorState.Order;

    // Update order details and representative actor state.
    order.OrderUpdatedDateTimeUtc = DateTime.UtcNow;
    order.OrderState = OrderState.Complete;
    actorState.UpdateOrder(order);

    await Task.WhenAll(
      _orderStateRepository.CheckoutOrderAsync(order),
      _orderPubSubRepository.PublishOrderEvent(order),
      StateManager.SetStateAsync(DaprComponents.OrderActorStateStore, actorState));

    Logger.LogDebug("MarkOrderAsCompletedAsync end. OrderId: {orderId}", orderId);
  }

  private async Task<OrderActorState> GetActorStateAsync()
  {
    var tryGetActorState = await StateManager.TryGetStateAsync<OrderActorState>(DaprComponents.OrderActorStateStore);
    if (!tryGetActorState.HasValue)
    {
      throw new OrderNotFoundException();
    }

    return tryGetActorState.Value;
  }
}