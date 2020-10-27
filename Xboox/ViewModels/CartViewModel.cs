using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models;

namespace Xboox.ViewModels
{
    public class CartViewModel : OrderItemsBase
    {
        public string ProductImgLink { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid ProductId { get; set; }
    }
}