using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models;
using XbooxLibrary.Models.DataTable;

namespace Xboox.ViewModels
{
    public class OrderDetailsViewModel: OrderItemsBase
    {
        public string Imagelink { get; set; }
        public decimal Total { get; set; }
        public Coupons Coupon { get; set; }
    }
}