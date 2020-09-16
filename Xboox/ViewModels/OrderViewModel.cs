using ECPay.Payment.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;

namespace Xboox.ViewModels
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public string UserName { get; set; }
        public string EcpayOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string PurchaserName { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Road { get; set; }
        public string PurchaserAddress { get; set; }
        public string PurchaserEmail { get; set; }
        public string PurchaserPhone { get; set; }
        public string Discount { get; set; }
        public string Payment { get; set; }
        public bool Paid { get; set; }
        public bool Build { get; set; }
        public bool Remember { get; set; }
    }
}