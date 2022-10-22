﻿namespace Domain.Constants;

/// <summary>
/// A set of constants pointing to the dapr components.
/// </summary>
public static class DaprComponents
{
  /// <summary>
  /// The order state store component.
  /// </summary>
  public const string OrderStateStore = "orders";

  /// <summary>
  /// The order actor state store component.
  /// </summary>
  public const string OrderActorStateStore = "orderactorstate";

  /// <summary>
  /// The pubsub component. 
  /// </summary>
  public const string PubSub = "pubsub";
}