using System.ComponentModel.DataAnnotations;
using Dapr;
using Domain.Constants;
using GarbageCollector.Managers;
using Microsoft.AspNetCore.Mvc;

namespace GarbageCollector.Controllers;

/// <summary>
/// Exposes endpoints for interacting with order actor garbage collection.
/// </summary>
[Route("v1/garbagecollector")]
public class GarbageCollectorController : ControllerBase
{
  private readonly IGarbageCollectorManager _manager;
  private readonly ILogger<GarbageCollectorController> _logger;

  /// <summary>
  /// Instantiates a new instance of the OrderActorGarbageCollectorController class.
  /// </summary>
  /// <param name="manager">The manager.</param>
  /// <param name="logger">The logger.</param>
  public GarbageCollectorController(
    IGarbageCollectorManager manager,
    ILogger<GarbageCollectorController> logger)
  {
    _manager = manager;
    _logger = logger;
  }

  /// <summary>
  /// Processes the encrypted order event.
  /// </summary>
  /// <param name="orderEvent">The order event.</param>
  [HttpPost]
  [Route("subscribe")]
  [Topic(DaprComponents.PubSub, "order-events")]
  public async Task<IActionResult> ProcessEventAsync([Required][FromBody] CloudEvent<string> orderEvent)
  {
    _logger.LogInformation("ProcessEventAsync start");
    await _manager.ProcessOrderEventAsync(orderEvent.Data);
    _logger.LogInformation("ProcessEventAsync end");
    return Ok();
  }
}