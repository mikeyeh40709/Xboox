using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.ViewModels
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public string PurchaserName { get; set; }
        public string PurchaserAddress { get; set; }
        public string PurchaserEmail { get; set; }
        public string PurchaserPhone { get; set; }
        // Details
        public int StateId { get; set; }
    }
}