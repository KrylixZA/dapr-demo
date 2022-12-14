using Domain.Models;

namespace Infrastructure.Actors;

/// <summary>
/// Represents the current state of the actor.
/// </summary>
public class OrderActorState
{
  /// <summary>
  /// The UTC date and time when the actor was created.
  /// </summary>
  public DateTime ActorCreatedDateTimeUtc { get; set; } = default!;

  /// <summary>
  /// The UTC date and time when the actor was last updated.
  /// </summary>
  public DateTime? ActorUpdatedDateTimeUtc { get; set; }

  /// <summary>
  /// The details of the order.
  /// </summary>
  public Order Order { get; set; } = default!;

  /// <summary>
  /// Updates the order details in the actor state, as well as the <see cref="ActorUpdatedDateTimeUtc"/> field.
  /// </summary>
  /// <param name="order">The updated order.</param>
  public void UpdateOrder(Order order)
  {
    Order = order;
    ActorUpdatedDateTimeUtc = order.OrderUpdatedDateTimeUtc;
  }
}