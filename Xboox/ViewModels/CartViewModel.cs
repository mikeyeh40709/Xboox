using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;

namespace Xboox.ViewModels
{
    public class CartViewModel
    {
        //public Guid Id { get; set; }
     //   public List<CartItems> CartList { get; set; }
        public string ProductImgLink { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        ////public string MyProperty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice{get;set;}

        public Guid ProductId { get; set; }
        


    }
}