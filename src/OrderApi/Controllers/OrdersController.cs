using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using OrderApi.Managers;

namespace OrderApi.Controllers;

/// <summary>
/// Exposes endpoints for interacting with orders in the state store.
/// </summary>
[ApiController]
[Route("v1/orders")]
public class OrdersController : ControllerBase
{
  private readonly IOrderManager _orderManager;
  private readonly ILogger<OrdersController> _logger;

  /// <summary>
  /// Instantiates a new instace of the orders controller class.
  /// </summary>
  /// <param name="orderManager">The order manager.</param>
  /// <param name="logger">The logger.</param>
  public OrdersController(IOrderManager orderManager, ILogger<OrdersController> logger)
  {
    _orderManager = orderManager;
    _logger = logger;
  }

  /// <summary>
  /// Persists a new order to the state storage.
  /// </summary>
  /// <remarks>
  /// As this is all part of a demo, the following will happen:
  /// 1. The order details will be written to a "orders" bucket as a JSON payload to the redis container.
  /// 2. A virtual actor with the order identifier as part of its name will be written to the redis container.
  /// </remarks>
  /// <param name="order">The details of the order.</param>
  [HttpPost]
  public async Task<IActionResult> CreateOrderAsync([Required][FromBody] Order order)
  {
    _logger.LogInformation("CreateOrderAsync start. OrderId: {orderId}", order.OrderId);
    await _orderManager.CreateOrderAsync(order);
    _logger.LogInformation("CreateOrderAsync end. OrderId: {orderId}", order.OrderId);
    return Ok();
  }

  /// <summary>
  /// Begins the checkout process for an order.
  /// </summary>
  /// <remarks>
  /// Because an order is represented by a virtual actor, this will initiate the following changes:
  /// 1. The order updated date time UTC field will be set with the current UTC time.
  /// 2. The actor state for the order will be updated.
  /// 3. A message will be published to a "events" exchange in RabbitMQ.
  /// </remarks>
  /// <param name="orderId">The order unique identifier.</param>
  [HttpPost]
  [Route("checkout/{orderId}")]
  public async Task<IActionResult> CheckoutOrderAsync([Required][FromRoute] Guid orderId)
  {
    _logger.LogInformation("CheckoutOrderAsync start. OrderId: {orderId}", orderId);
    await _orderManager.CheckoutOrderAsync(orderId);
    _logger.LogInformation("CheckoutOrderAsync start. OrderId: {orderId}", orderId);
    return Ok();
  }

  /// <summary>
  /// Completes an order.
  /// </summary>
  /// <remarks>
  /// Because an order is represented by a virtual actor, this will initiate the following changes:
  /// 1. The order updated date time UTC field will be set with the current UTC time.
  /// 2. The actor state for the order will be updated.
  /// 3. A message will be published to a "checkout" exchange in RabbitMQ.
  /// </remarks>
  /// <param name="orderId">The order unique identifier.</param>
  [HttpPost]
  [Route("complete/{orderId}")]
  public async Task<IActionResult> CompleteOrderAsync([Required][FromRoute] Guid orderId)
  {
    _logger.LogInformation("CompleteOrderAsync start. OrderId: {orderId}", orderId);
    await _orderManager.CompleteOrderAsync(orderId);
    _logger.LogInformation("CompleteOrderAsync start. OrderId: {orderId}", orderId);
    return Ok();
  }

  /// <summary>
  /// Forcefully deactivates the actor from state.
  /// This will forcefully write the actor's current state to the state store and remove the actor from memory.
  /// </summary>
  /// <remarks>
  /// The following steps will occur:
  /// 1. The actor will update it's own state into that of a "deactivated" state.
  /// 2. The actor will be removed from memory and it's state persisted to disk.
  /// </remarks>
  /// <param name="orderId">The order unique identifier.</param>
  [HttpDelete]
  [Route("deactivate/{orderId}")]
  public async Task<IActionResult> DeactivateOrderActorAsync([Required][FromRoute] Guid orderId)
  {
    _logger.LogInformation("DeactivateOrderActorAsync start. OrderId: {orderId}", orderId);
    await _orderManager.DeactivateOrderActorAsync(orderId);
    _logger.LogInformation("DeactivateOrderActorAsync end. OrderId: {orderId}", orderId);
    return Ok();
  }
}