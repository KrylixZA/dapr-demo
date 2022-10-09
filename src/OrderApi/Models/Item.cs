using System;

namespace OrderApi.Models
{
    public class Item
    {
        public Guid ItemId { get; set; } = Guid.NewGuid();

        public string ItemName { get; set; } = string.Empty;

        public string ItemDescription { get; set; } = string.Empty;

        public double Cost { get; set; } = default;
    }
}