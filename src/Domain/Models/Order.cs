using System.ComponentModel.DataAnnotations;
using Domain.Attributes;
using Domain.Enums;

namespace Domain.Models;

/// <summary>
/// Defines an individual order.
/// </summary>
public class Order
{
  /// <summary>
  /// The order unique identifier.
  /// </summary>
  [Required]
  //[MustNotBeDefault]
  public Guid OrderId { get; set; }

  /// <summary>
  /// The UTC date and time when the order was made.
  /// </summary>
  [Required]
  //[MustNotBeDefault]
  public DateTime OrderCreatedDateTimeUtc { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// The UTC date and time when the order was updated.
  /// </summary>
  public DateTime? OrderUpdatedDateTimeUtc { get; set; }

  /// <summary>
  /// The current state of the order.
  /// </summary>
  [Required]
  public OrderState OrderState { get; set; } = OrderState.Created;

  /// <summary>
  /// The list of items ordered.
  /// </summary>
  [Required]
  public IEnumerable<Item> Items { get; set; } = default!;
}