using Application.Actors;
using Dapr.Actors;
using Dapr.Actors.Client;
using Domain.Models;
using Infrastructure.Actors;

namespace OrderApi.Managers;

/// <summary>
/// Implements a contract for managing orders.
/// </summary>
public class OrderManager : IOrderManager
{
  private readonly ILogger<OrderManager> _logger;

  /// <summary>
  /// Instantiates a new instance of the OrderManager class.
  /// </summary>
  /// <param name="logger">The logger.</param>
  public OrderManager(ILogger<OrderManager> logger)
  {
    _logger = logger;
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
    await orderActor.CheckoutOrderAsync(orderId);
    _logger.LogDebug("CheckoutOrderAsync end. OrderId: {orderId}", orderId);
  }

  /// <inheritdoc/>
  public async Task CompleteOrderAsync(Guid orderId)
  {
    _logger.LogDebug("CompleteOrderAsync start. OrderId: {orderId}", orderId);
    var actorId = new ActorId(orderId.ToString());
    var orderActor = ActorProxy.Create<IOrderActor>(actorId, OrderActor.ActorType);
    await orderActor.MarkOrderAsCompletedAsync(orderId);
    _logger.LogDebug("CompleteOrderAsync end. OrderId: {orderId}", orderId);
  }
}