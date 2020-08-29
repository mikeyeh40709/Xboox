using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.ViewModels
{
    public class ProductDetailViewModel
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public string imgLink { get; set; }
        public string Intro { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
    }
}
