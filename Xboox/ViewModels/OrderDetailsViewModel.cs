using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.ViewModels
{
    public class OrderDetailsViewModel
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string Imagelink { get; set; }
        public string Quantity { get; set; }
        public decimal Total { get; set; }
    }
}