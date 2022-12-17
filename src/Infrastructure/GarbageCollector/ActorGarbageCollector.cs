using Application.GarbageCollector;
using Microsoft.Extensions.Logging;

namespace Infrastructure.GarbageCollector;

/// <summary>
/// Implements a contract for garbage collecting actors.
/// </summary>
public class ActorGarbageCollector : IActorGarbageCollector
{
  private readonly ILogger<ActorGarbageCollector> _logger;
  private readonly IHttpClientFactory _httpClientFactory;

  /// <summary>
  /// Instantiates a new instance of the ActorGarbageCollector class.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="httpClientFactory">The HTTP client factory.</param>
  public ActorGarbageCollector(ILogger<ActorGarbageCollector> logger, IHttpClientFactory httpClientFactory)
  {
    _logger = logger;
    _httpClientFactory = httpClientFactory;
  }

  /// <inheritdoc/>
  public async Task GarbageCollectActorAsync(string actorId, string actorType)
  {
    _logger.LogDebug("GarbageCollectActorAsync start. ActorId: {actorId}. ActorType: {actorType}", actorId, actorType);
    var client = _httpClientFactory.CreateClient();
    var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5000/actors/{actorType}/{actorId}");
    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
    _logger.LogDebug("GarbageCollectActorAsync end. ActorId: {actorId}. ActorType: {actorType}", actorId, actorType);
  }
}

