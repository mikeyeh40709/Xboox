using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class ProductUnitViewModel
    {

        [StringLength(50)]
        public string Name { get; set; }

        public int UnitInStock { get; set; }

        public decimal Price { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        [StringLength(50)]
        public string Publisher { get; set; }

        [StringLength(10)]
        public DateTime PublishedDate { get; set; }

        public string Intro { get; set; }
        public string Language { get; set; }

        public Guid CategoryId { get; set; }


        [StringLength(50)]
        public string CategorName { get; set; }

        public string Specification { get; set; }

        [System.Web.Mvc.AllowHtml]
        public string Description { get; set; }

 





    }
}