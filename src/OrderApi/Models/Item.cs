using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderApi.Models
{
    /// <summary>
    /// Defines an individual item.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The item unique identifier.
        /// </summary>
        [Required]
        [JsonPropertyName("itemId")]
        public Guid ItemId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The name of the item.
        /// </summary>
        [Required]
        [JsonPropertyName("name")]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// The item description.
        /// </summary>
        [Required]
        [JsonPropertyName("description")]
        public string ItemDescription { get; set; } = string.Empty;

        /// <summary>
        /// The cost of the item.
        /// </summary>
        [Required]
        [JsonPropertyName("cost")]
        public double Cost { get; set; } = default;
    }
}