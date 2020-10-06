using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class CreateListViewModel:ProductUnitViewModel
    {
        public IEnumerable<ProductListViewModel> productListViews { get; set; }

        [Required(ErrorMessage = "產品名稱為必要項!")]
        [StringLength(50)]
        new public string Name { get; set; }

        [Required(ErrorMessage = "產品價格為必要項!")]
        new public decimal Price { get; set; }
        public int ProductImgId { get; set; }

        public Guid ProductId { get; set; }


        public IEnumerable<CategoryViewModel> CategoryViewModels { get; set; }


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
            public string Name { get; set; }

            public Guid CategoryId { get; set; }
        }

    }
}