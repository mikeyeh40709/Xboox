using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.ViewModels
{
    public class BookCategoryViewModel
    {
        public string imgLink { get; set; }
        public List<string> imgLinks { get; set; }
        public string Tags { get; set; }
        public decimal Price { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
    }
}