using Application.Helpers;
using Domain.Enums;
using Domain.Models;
using GarbageCollector.GarbageCollector;
using Infrastructure.Actors;

namespace GarbageCollector.Managers;

/// <summary>
/// Implements a contract for managing order events and garbage collecting any completed orders.
/// </summary>
public class GarbageCollectorManager : IGarbageCollectorManager
{
  private readonly IAesEncryptionHelper _aesEncryptionHelper;
  private readonly IActorGarbageCollector _actorGarbageCollector;
  private readonly ILogger<GarbageCollectorManager> _logger;

  /// <summary>
  /// Instantiates a new instance of the GarbageCollectorManager class.
  /// </summary>
  /// <param name="aesEncryptionHelper">The AES encryption helper.</param>
  /// <param name="actorGarbageCollector">The actor garbage collector.</param>
  /// <param name="logger">The logger.</param>
  public GarbageCollectorManager(
    IAesEncryptionHelper aesEncryptionHelper,
    IActorGarbageCollector actorGarbageCollector,
    ILogger<GarbageCollectorManager> logger)
  {
    _aesEncryptionHelper = aesEncryptionHelper;
    _actorGarbageCollector = actorGarbageCollector;
    _logger = logger;
  }

  /// <inheritdoc/>
  public async Task ProcessOrderEventAsync(string encryptedOrderEvent)
  {
    _logger.LogDebug("ProcessOrderEventAsync start");
    var order = await _aesEncryptionHelper.DecryptToObjectAsync<Order>(encryptedOrderEvent);
    if (order.OrderState != OrderState.Complete)
    {
      _logger.LogWarning("ProcessOrderEventAsync end. Event is not a complete event.");
      return;
    }

    await _actorGarbageCollector.GarbageCollectActorAsync(order.OrderId.ToString(), OrderActor.ActorType);
    _logger.LogDebug("ProcessOrderEventAsync end");
  }
}

