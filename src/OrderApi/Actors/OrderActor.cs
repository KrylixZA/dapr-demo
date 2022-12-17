using Application.Repositories;
using Dapr.Actors.Runtime;
using Domain.Constants;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace OrderApi.Actors;

/// <summary>
/// Represents an order as a virtual actor.
/// </summary>
[Actor(TypeName = nameof(OrderActor))]
public class OrderActor : Actor, IOrderActor
{
  private readonly IOrderStateRepository _orderStateRepository;
  private readonly IOrderPubSubRepository _orderPubSubRepository;

  /// <summary>
  /// The actor type.
  /// </summary>
  public static string ActorType => nameof(OrderActor);

  /// <summary>
  /// Initializes a new instance of the OrderActor.
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

    var actorState = OrderActorState.CreateActorState(order);

    await Task.WhenAll(
      _orderStateRepository.SaveOrderAsync(order),
      _orderPubSubRepository.PublishOrderEvent(order),
      StateManager.SetStateAsync(DaprComponents.OrderActorStateStore, actorState));

    Logger.LogDebug("CreateOrderAsync end. OrderId: {orderId}", order.OrderId);
  }

  /// <inheritdoc />
  public async Task CheckoutOrderAsync()
  {
    Logger.LogDebug("CheckoutOrderAsync start");

    var actorState = await GetActorStateAsync();
    var order = actorState.Order;

    // Update order details and representative actor state.
    order.OrderUpdatedDateTimeUtc = DateTime.UtcNow;
    order.OrderState = OrderState.CheckOut;
    actorState = OrderActorState.UpdateActorState(actorState, order);

    await Task.WhenAll(
      _orderStateRepository.SaveOrderAsync(order),
      _orderPubSubRepository.PublishOrderEvent(order),
      StateManager.SetStateAsync(DaprComponents.OrderActorStateStore, actorState));

    Logger.LogDebug("CheckoutOrderAsync end");
  }

  /// <inheritdoc />
  public async Task MarkOrderAsCompletedAsync()
  {
    Logger.LogDebug("MarkOrderAsCompletedAsync start");

    var actorState = await GetActorStateAsync();
    var order = actorState.Order;

    // Update order details and representative actor state.
    order.OrderUpdatedDateTimeUtc = DateTime.UtcNow;
    order.OrderState = OrderState.Complete;
    actorState = OrderActorState.UpdateActorState(actorState, order);

    await Task.WhenAll(
      _orderStateRepository.SaveOrderAsync(order),
      _orderPubSubRepository.PublishOrderEvent(order),
      StateManager.SetStateAsync(DaprComponents.OrderActorStateStore, actorState));

    Logger.LogDebug("MarkOrderAsCompletedAsync end");
  }

  /// <inheritdoc />
  public async Task DeactivateActorAsync()
  {
    Logger.LogDebug("DeactivateActorAsync start");
    var actorState = await GetActorStateAsync();
    actorState = OrderActorState.MarkActorStateAsDeactivated(actorState);
    await StateManager.SetStateAsync(DaprComponents.OrderActorStateStore, actorState);
    Logger.LogDebug("DeactivateActorAsync end");
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