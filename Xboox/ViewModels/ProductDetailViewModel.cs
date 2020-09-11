using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;

namespace Xboox.ViewModels
{
    public class ProductDetailViewModel
    {
        public int UnitInStock { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public string ImgLink { get; set; }
        public string Intro { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public string ProductId { get; set; }
        public string TagId { get; set; }
        public string TagName { get; set; }
        public List<string> ImgLinks { get; set; }
        //public string Category { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
