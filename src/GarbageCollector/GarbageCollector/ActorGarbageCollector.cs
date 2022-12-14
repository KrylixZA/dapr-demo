namespace GarbageCollector.GarbageCollector;

/// <summary>
/// Implements a contract for garbage collecting actors.
/// </summary>
public class ActorGarbageCollector : IActorGarbageCollector
{
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly ILogger<ActorGarbageCollector> _logger;

  /// <summary>
  /// Instantiates a new instance of the ActorGarbageCollector class.
  /// </summary>
  /// <param name="httpClientFactory">The HTTP client factory.</param>
  /// <param name="logger">The logger.</param>
  public ActorGarbageCollector(IHttpClientFactory httpClientFactory, ILogger<ActorGarbageCollector> logger)
  {
    _httpClientFactory = httpClientFactory;
    _logger = logger;
  }

  /// <inheritdoc/>
  public async Task GarbageCollectActorAsync(string actorId, string actorType)
  {
    _logger.LogDebug("GarbageCollectActorAsync start. ActorId: {actorId}. ActorType: {actorType}", actorId, actorType);

    var client = _httpClientFactory.CreateClient();
    var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost/actors/{actorType}/{actorId}");
    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();

    _logger.LogDebug("GarbageCollectActorAsync end. ActorId: {actorId}. ActorType: {actorType}", actorId, actorType);
  }
}

