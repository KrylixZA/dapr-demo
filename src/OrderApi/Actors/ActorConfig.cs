using Dapr.Actors;

namespace OrderApi.Actors;

/// <summary>
/// Defines all the properties of Actor configuration in Dapr.
/// See more at: https://docs.dapr.io/developing-applications/building-blocks/actors/howto-actors/#actor-runtime-configuration
/// </summary>
public class ActorConfig
{
  /// <summary>
  /// The timeout before deactivating an idle actor.
  /// Checks for timeouts occur every actorScanInterval interval.
  /// Default: 60 minutes
  /// </summary>
  public int ActorIdleTimeout { get; set; } = 60;

  /// <summary>
  /// The duration which specifies how often to scan for actors to deactivate idle actors.
  /// Actors that have been idle longer than actor_idle_timeout will be deactivated.
  /// Default: 30 seconds
  /// </summary>
  public int ActorScanInterval { get; set; } = 30;

  /// <summary>
  /// The duration when in the process of draining rebalanced actors.
  /// This specifies the timeout for the current active actor method to finish.
  /// If there is no current actor method call, this is ignored.
  /// Default: 60 seconds
  /// </summary>
  public int DrainOngoingCallTimeout { get; set; } = 60;

  /// <summary>
  /// If true, Dapr will wait for drainOngoingCallTimeout duration to allow a current actor call to complete before trying to deactivate an actor.
  /// Default: true
  /// </summary>
  public bool DrainRebalancedActors { get; set; } = true;

  /// <summary>
  /// Configure the number of partitions for actor’s reminders.
  /// If not provided, all reminders are saved as a single record in actor’s state store.
  /// Default: 0
  /// </summary>
  public int RemindersStoragePartitions { get; set; } = 0;

  /// <summary>
  /// Configure the reentrancy behavior for an actor.
  /// If not provided, reentrancy is disabled.
  /// Default: disabled
  /// Default: false
  /// </summary>
  public ActorReentrancyConfig? ReentrancyConfig { get; set; }
}