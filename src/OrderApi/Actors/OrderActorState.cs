using Domain.Models;

namespace OrderApi.Actors;

/// <summary>
/// Represents the current state of the actor.
/// </summary>
public class OrderActorState
{
  /// <summary>
  /// The UTC date and time when the actor was created.
  /// </summary>
  public DateTime ActorCreatedDateTimeUtc { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// The UTC date and time when the actor was last updated.
  /// </summary>
  public DateTime? ActorUpdatedDateTimeUtc { get; set; }

  /// <summary>
  /// The UTC date and time when the actor was deactivated.
  /// </summary>
  public DateTime? ActorDeactivatedDateTimeUtc { get; set; }

  /// <summary>
  /// The UTC date and time when the actor was reactivated.
  /// </summary>
  public DateTime? ActorReactivatedDateTimeUtc { get; set; }

  /// <summary>
  /// Represents the current actor state.
  /// </summary>
  public ActorState CurrentActorState { get; set; } = ActorState.Active;

  /// <summary>
  /// The details of the order.
  /// </summary>
  public Order Order { get; set; } = default!;

  /// <summary>
  /// Creates a new instance of the actor state from the order.
  /// </summary>
  /// <param name="order">The order.</param>
  /// <returns>An instance of the actor state based off the order.</returns>
  public static OrderActorState CreateActorState(Order order)
  {
    return new OrderActorState
    {
      Order = order,
      ActorCreatedDateTimeUtc = DateTime.UtcNow
    };
  }

  /// <summary>
  /// Updates the order details in the actor state, as well as the <see cref="ActorUpdatedDateTimeUtc"/> field.
  /// </summary>
  /// <param name="orderActorState">The actor state.</param>
  /// <param name="updatedOrder">The updated order.</param>
  public static OrderActorState UpdateActorState(OrderActorState orderActorState, Order updatedOrder)
  {
    orderActorState.Order = updatedOrder;
    orderActorState.ActorUpdatedDateTimeUtc = updatedOrder.OrderUpdatedDateTimeUtc;
    return orderActorState;
  }

  /// <summary>
  /// Updates the order details in the actor state, as well as the <see cref="ActorUpdatedDateTimeUtc"/> and <see cref="ActorDeactivatedDateTimeUtc"/> fields.
  /// </summary>
  /// <param name="orderActorState">The actor state.</param>
  public static OrderActorState MarkActorStateAsDeactivated(OrderActorState orderActorState)
  {
    var utcNow = DateTime.UtcNow;
    orderActorState.ActorDeactivatedDateTimeUtc = utcNow;
    orderActorState.CurrentActorState = ActorState.Deactivated;

    return orderActorState;
  }
}