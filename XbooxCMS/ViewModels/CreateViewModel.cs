using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using XbooxCMS.Models;

namespace XbooxCMS.ViewModels
{
    public class CreateViewModel
    {

        //一般產品欄位
        public Product Products { get; set; }


        //foreach Tags 標籤
        public IEnumerable<Tags> Tags { get; set; }

        //Category 
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<TagViewModel> TagViews { get; set; }
        public IEnumerable<CategoryViewModel> categoryViews { get; set; }

        public string Name { get; set; }
        public CreateViewModel()
        {

        }



        //[StringLength(50)]
        //public string Name { get; set; }

        //public int Quantity { get; set; }

        //public decimal Price { get; set; }

        //[StringLength(50)]
        //public string ISBN { get; set; }

        //public int ProductImgId { get; set; }

        //[StringLength(50)]
        //public string Author { get; set; }

        //[StringLength(50)]
        //public string Publisher { get; set; }

        //[StringLength(10)]
        //public string PublishedDate { get; set; }

        //public string Intro { get; set; }


        //[StringLength(50)]
        //public string CategorName { get; set; }


        //[StringLength(50)]
        //public string TagName { get; set; }

    }
}