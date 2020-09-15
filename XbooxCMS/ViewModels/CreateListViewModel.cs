using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XbooxCMS.ViewModels
{
    public class CreateListViewModel
    {

        //Product
        [StringLength(50)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        public int ProductImgId { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        [StringLength(50)]
        public string Publisher { get; set; }

        [StringLength(10)]
        public string PublishedDate { get; set; }

        public string Intro { get; set; }


        [StringLength(50)]
        public string CategorName { get; set; }


        [StringLength(50)]
        public string TagName { get; set; }



    }
}