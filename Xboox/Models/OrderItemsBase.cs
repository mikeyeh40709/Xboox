using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.Models
{
    public partial class OrderItemsBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}