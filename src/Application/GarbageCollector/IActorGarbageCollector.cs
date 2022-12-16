namespace Application.GarbageCollector;

/// <summary>
/// Exposes a contract for garbage collecting actors.
/// </summary>
public interface IActorGarbageCollector
{
  /// <summary>
  /// Garbage collects the actor. This will forcefully deactivate the actor from working memory.
  /// See more here: https://docs.dapr.io/reference/api/actors_api/#deactivate-actor
  /// </summary>
  /// <param name="actorId">The actor identifer.</param>
  /// <param name="actorType">The actor type.</param>
  Task GarbageCollectActorAsync(string actorId, string actorType);
}