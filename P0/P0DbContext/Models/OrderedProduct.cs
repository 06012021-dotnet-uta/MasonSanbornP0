using System;
using System.Collections.Generic;

#nullable disable

namespace P0DbContext.Models
{
    public partial class OrderedProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int NumberOrdered { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
