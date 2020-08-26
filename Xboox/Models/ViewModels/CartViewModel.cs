using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XbooxCMS.Models.ViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public string ProductImg { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string MyProperty { get; set; }
        public decimal Total { get; set; }
    }
}