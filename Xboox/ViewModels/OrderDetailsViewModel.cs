using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.ViewModels
{
    public class OrderDetailsViewModel
    {
        public string Imagelink { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public Guid? Discount { get; set; }
    }
}