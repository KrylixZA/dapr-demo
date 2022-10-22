using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
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
  [MustNotBeDefault]
  [JsonPropertyName("orderId")]
  public Guid OrderId { get; set; } = default!;

  /// <summary>
  /// The UTC date and time when the order was made.
  /// </summary>
  [Required]
  [MustNotBeDefault]
  [JsonPropertyName("orderCreatedDateTimeUtc")]
  public DateTime OrderCreatedDateTimeUtc { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// The UTC date and time when the order was updated.
  /// </summary>
  [JsonPropertyName("orderUpdatedDateTimeUtc")]
  public DateTime? OrderUpdatedDateTimeUtc { get; set; }

  /// <summary>
  /// The current state of the order.
  /// </summary>
  [Required]
  [JsonPropertyName("orderState")]
  public OrderState OrderState { get; set; } = OrderState.Created;

  /// <summary>
  /// The list of items ordered.
  /// </summary>
  [Required]
  [JsonPropertyName("items")]
  public IEnumerable<Item> Items { get; set; } = default!;
}