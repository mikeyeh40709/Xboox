using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class TestViewModel:ProductUnitViewModel
    {
     
        public string TagName { get; set; }


        public Guid TagId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
    }
}