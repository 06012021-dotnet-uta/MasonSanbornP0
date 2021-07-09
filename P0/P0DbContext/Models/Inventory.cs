using System;
using System.Collections.Generic;

#nullable disable

namespace P0DbContext.Models
{
    public partial class Inventory
    {
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public int NumberProducts { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }
    }
}
