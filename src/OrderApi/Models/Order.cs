using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace OrderApi.Models
{
    /// <summary>
    /// Defines an individual order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The order unique identifier.
        /// </summary>
        [Required]
        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The UTC date and time when the order was made.
        /// </summary>
        [Required]
        [JsonPropertyName("orderDateTime")]
        public DateTime OrderDateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The list of items ordered.
        /// </summary>
        [Required]
        [JsonPropertyName("items")]
        public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();
    }
}