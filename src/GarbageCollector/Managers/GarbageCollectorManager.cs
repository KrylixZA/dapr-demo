using Application.Helpers;
using Dapr.Actors;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Actors;

namespace GarbageCollector.Managers;

/// <summary>
/// Implements a contract for managing order events and garbage collecting any completed orders.
/// </summary>
public class GarbageCollectorManager : IGarbageCollectorManager
{
  private readonly ILogger<GarbageCollectorManager> _logger;
  private readonly IAesEncryptionHelper _aesEncryptionHelper;
  private readonly IHttpClientFactory _httpClientFactory;

  /// <summary>
  /// Instantiates a new instance of the GarbageCollectorManager class.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="aesEncryptionHelper">The AES encryption helper.</param>
  /// <param name="httpClientFactory">The HTTP client factory.</param>
  public GarbageCollectorManager(
    ILogger<GarbageCollectorManager> logger,
    IAesEncryptionHelper aesEncryptionHelper,
    IHttpClientFactory httpClientFactory)
  {
    _logger = logger;
    _aesEncryptionHelper = aesEncryptionHelper;
    _httpClientFactory = httpClientFactory;
  }

  /// <inheritdoc/>
  public async Task ProcessOrderEventAsync(string encryptedOrderEvent)
  {
    _logger.LogDebug("ProcessOrderEventAsync start");
    var order = await _aesEncryptionHelper.DecryptToObjectAsync<Order>(encryptedOrderEvent);
    if (order.OrderState != OrderState.Complete)
    {
      _logger.LogDebug("ProcessOrderEventAsync end. Event is not a complete event.");
      return;
    }

    await InvokeDeactivateOrderActorAsync(order.OrderId);
    _logger.LogDebug("ProcessOrderEventAsync end");
  }

  private async Task InvokeDeactivateOrderActorAsync(Guid orderId)
  {
    var client = _httpClientFactory.CreateClient();
    var request = new HttpRequestMessage(HttpMethod.Delete, $"http://orderapi:5000/v1/orders/deactivate/{orderId}");
    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
  }
}

