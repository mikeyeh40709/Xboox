using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.Models.ViewModels
{
    public class CartViewModel
    {
        //public Guid Id { get; set; }
        public string ProductImgLink { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        //public string MyProperty { get; set; }
        public decimal Total { get; set; }
    }
}