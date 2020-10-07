using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class ProductListViewModel:ProductUnitViewModel
    {
        //productListViewModel
        [StringLength(50)]
        public string TagName { get; set; }


        public Guid TagId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public string ProductImgId { get; set; }
    }
}