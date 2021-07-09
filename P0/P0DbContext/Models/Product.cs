using System;
using System.Collections.Generic;

#nullable disable

namespace P0DbContext.Models
{
    public partial class Product
    {
        public Product()
        {
            Inventories = new HashSet<Inventory>();
            OrderedProducts = new HashSet<OrderedProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
