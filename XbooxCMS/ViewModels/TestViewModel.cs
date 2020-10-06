using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class TestViewModel:ProductUnitViewModel
    {
        //CreateDataModel
        public List<Guid> PostedTagIds { get; set; }

        public XbooxLibrary.Models.DataTable.ProductTags ProductTags { get; set; }
        public string ProductImgId { get; set; }

        public Guid? ProductId { get; set; }
    }
}