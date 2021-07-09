using System;
using System.Collections.Generic;

#nullable disable

namespace P0DbContext.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderedProducts = new HashSet<OrderedProduct>();
        }

        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public int CustomerId { get; set; }
        public int LocationId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
