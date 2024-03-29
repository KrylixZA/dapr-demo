﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Attributes;

namespace Domain.Models;

/// <summary>
/// Defines an individual item.
/// </summary>
public class Item
{
  /// <summary>
  /// The item unique identifier.
  /// </summary>
  [Required]
  [MustNotBeDefault]
  [JsonPropertyName("itemId")]
  public Guid ItemId { get; set; } = default!;

  /// <summary>
  /// The name of the item.
  /// </summary>
  [Required]
  [JsonPropertyName("name")]
  public string ItemName { get; set; } = default!;

  /// <summary>
  /// The item description.
  /// </summary>
  [Required]
  [JsonPropertyName("description")]
  public string ItemDescription { get; set; } = default!;

  /// <summary>
  /// The cost of the item. This must range from 1.00 to 1.7976931348623157E+308.
  /// </summary>
  [Required]
  [JsonPropertyName("cost")]
  [Range(1d, double.MaxValue)]
  public double Cost { get; set; } = default!;
}