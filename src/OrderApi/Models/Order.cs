using System;
using System.Linq;

namespace OrderApi.Models
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();

        public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();
    }
}