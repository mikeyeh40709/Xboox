using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XbooxCMS.ViewModels
{
    public class CreateListViewModel
    {

        public IEnumerable<ProductListViewModel> productListViews { get; set; }

            [Required]
            [StringLength(50)]
            public string Name { get; set; }

           public string Specification { get; set; }

           public int UnitInStock { get; set; }

            public decimal Price { get; set; }

            [StringLength(50)]
            public string ISBN { get; set; }

            public int ProductImgId { get; set; }

            [StringLength(50)]
            public string Author { get; set; }

            [StringLength(50)]
            public string Publisher { get; set; }

            [StringLength(10)]
            public DateTime PublishedDate { get; set; }

            public string Intro { get; set; }

            [System.Web.Mvc.AllowHtml]
            public string Description { get; set; }
            public Guid CategoryId { get; set; }

            //Product
        

        public   IEnumerable<CategoryViewModel> CategoryViewModels { get; set; }


        //[StringLength(50)]
        //public string TagName { get; set; }

        //CheckBoxList中的選項清單
        public IEnumerable<TagViewModel> Tags { get; set; }

        //CheckBoxList被選中的checkbox
        public IEnumerable<TagViewModel> SelectedTags { get; set; }

        //CheckBoxList的名稱，也是被勾選資料Post回Server時Data binding之目標物件
        public List<Guid> PostedTagIds { get; set; }


        public class CategoryViewModel
        {
            public string CategorName { get; set; }

            public Guid CategoryId { get; set; }
        }


       


    }
}