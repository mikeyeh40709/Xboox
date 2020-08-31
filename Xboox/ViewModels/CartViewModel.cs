using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;

namespace Xboox.Models.ViewModels
{
    public class CartViewModel
    {
        //public Guid Id { get; set; }
        public List<CartItmes> CartItems { get; set; }
        public string ProductImgLink { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        //public string MyProperty { get; set; }
        public decimal Price { get; set; }
    }
}