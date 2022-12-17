using Application.GarbageCollector;
using Dapr.Actors;
using Dapr.Actors.Client;
using Domain.Models;
using Infrastructure.GarbageCollector;
using OrderApi.Actors;

namespace OrderApi.Managers;

/// <summary>
/// Implements a contract for managing orders.
/// </summary>
public class OrderManager : IOrderManager
{
  private readonly ILogger<OrderManager> _logger;
  private readonly IActorGarbageCollector _actorGarbageCollector;

  /// <summary>
  /// Instantiates a new instance of the OrderManager class.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="actorGarbageCollector">The actor garbage collector.</param>
  public OrderManager(ILogger<OrderManager> logger, IActorGarbageCollector actorGarbageCollector)
  {
    _logger = logger;
    _actorGarbageCollector = actorGarbageCollector;
  }

  /// <inheritdoc/>
  public async Task CreateOrderAsync(Order order)
  {
    _logger.LogDebug("CreateOrderAsync start. OrderId: {orderId}", order.OrderId);
    var actorId = new ActorId(order.OrderId.ToString());
    var orderActor = ActorProxy.Create<IOrderActor>(actorId, OrderActor.ActorType);
    await orderActor.CreateOrderAsync(order);
    _logger.LogDebug("CreateOrderAsync end. OrderId: {orderId}", order.OrderId);
  }

  /// <inheritdoc/>
  public async Task CheckoutOrderAsync(Guid orderId)
  {
    _logger.LogDebug("CheckoutOrderAsync start. OrderId: {orderId}", orderId);
    var actorId = new ActorId(orderId.ToString());
    var orderActor = ActorProxy.Create<IOrderActor>(actorId, OrderActor.ActorType);
    await orderActor.CheckoutOrderAsync();
    _logger.LogDebug("CheckoutOrderAsync end. OrderId: {orderId}", orderId);
  }

  /// <inheritdoc/>
  public async Task CompleteOrderAsync(Guid orderId)
  {
    _logger.LogDebug("CompleteOrderAsync start. OrderId: {orderId}", orderId);
    var actorId = new ActorId(orderId.ToString());
    var orderActor = ActorProxy.Create<IOrderActor>(actorId, OrderActor.ActorType);
    await orderActor.MarkOrderAsCompletedAsync();
    _logger.LogDebug("CompleteOrderAsync end. OrderId: {orderId}", orderId);
  }

  /// <inheritdoc/>
  public async Task DeactivateOrderActorAsync(Guid orderId)
  {
    _logger.LogDebug("DeactivateOrderActorAsync start. OrderId: {orderId}", orderId);
    var actorId = new ActorId(orderId.ToString());
    var orderActor = ActorProxy.Create<IOrderActor>(actorId, OrderActor.ActorType);
    await orderActor.DeactivateActorAsync();
    await _actorGarbageCollector.GarbageCollectActorAsync(orderId.ToString(), OrderActor.ActorType);
    _logger.LogDebug("DeactivateOrderActorAsync end. OrderId: {orderId}", orderId);
  }
}