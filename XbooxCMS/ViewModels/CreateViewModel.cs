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


        //CheckBoxList中的選項清單
        public IEnumerable<Tags> Tags { get; set; }

        //CheckBoxList被選中的checkbox
        public IEnumerable<Tags> SelectedTags { get; set; }

        //CheckBoxList的名稱，也是被勾選資料Post回Server時Data binding之目標物件
        public List<Guid> PostedTagIds { get; set; }
        //Category 
        public IEnumerable<Category> Categories { get; set; }

        public ProductTags ProductTags { get; set; }

        public CreateViewModel()
        {
            Products = new Product();
            SelectedTags = new List<Tags>();
            PostedTagIds = new List<Guid>();
            ProductTags = new ProductTags();
            
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