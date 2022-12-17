namespace OrderApi.Actors;

/// <summary>
/// Defines an enumeration of valid states an actor can be in.
/// </summary>
public enum ActorState
{
  /// <summary>
  /// The actor is currently active.
  /// </summary>
  Active = 0,

  /// <summary>
  /// The actor is deactivated.
  /// </summary>
  Deactivated = 1,

  /// <summary>
  /// The actor has been reactivated and is now active again.
  /// </summary>
  Reactivated = 2
}