using System;
namespace Domain.Enums;

/// <summary>
/// A list of possible states that an order can be in.
/// </summary>
public enum OrderState
{
  /// <summary>
  /// The order has just been created.
  /// </summary>
  Created = 0,

  /// <summary>
  /// The checkout process has begun.
  /// </summary>
  CheckOut = 1,

  /// <summary>
  /// The order has been processed to completion.
  /// </summary>
  Complete = 2,

  /// <summary>
  /// The order was abandoned by the user.
  /// </summary>
  Abandoned = 3,

  /// <summary>
  /// An error occured during processing.
  /// </summary>
  Errored = 4
}