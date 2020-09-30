using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxLibrary.Models.DataTable;

namespace XbooxCMS.ViewModels
{
    public class OrderDetailsViewModel
    {

        public string Imagelink { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public Coupons Coupon { get; set; }
    }
}