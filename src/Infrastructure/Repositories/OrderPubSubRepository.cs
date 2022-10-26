using System.Text.Json;
using Application.Helpers;
using Application.Repositories;
using Dapr.Client;
using Domain.Constants;
using Domain.Models;

namespace Infrastructure.Repositories;

/// <summary>
/// Implements a contract for interacting with order pub/sub resources.
/// </summary>
public class OrderPubSubRepository : IOrderPubSubRepository
{
  private readonly DaprClient _daprClient;
  private readonly IAesEncryptionHelper _aesEncryptionHelper;

  /// <summary>
  /// Instantiates a new instance of the order pub/sub repository class.
  /// </summary>
  /// <param name="aesEncryptionHelper">The AES encryption helper.</param>
  public OrderPubSubRepository(IAesEncryptionHelper aesEncryptionHelper)
  {
    _daprClient = new DaprClientBuilder().Build();
    _aesEncryptionHelper = aesEncryptionHelper;
  }

  /// <inheritdoc/>
  public async Task PublishOrderForCheckoutAsync(Order order)
  {
    var encryptedPayload = await _aesEncryptionHelper.EncryptStringAsync(JsonSerializer.Serialize(order));
    await _daprClient.PublishEventAsync<string>(
      DaprComponents.PubSub,
      "checkout",
      encryptedPayload);
  }
}