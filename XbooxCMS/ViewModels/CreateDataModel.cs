﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class CreateDataModel
    {

        public string Name { get; set; }
        public int UnitInStock { get; set; }
        public decimal Price { get; set; }

        public string ISBN { get; set; }

        public string ProductImgId { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Intro { get; set; }

        public Guid CategoryId { get; set; }
        public Guid? ProductId { get; set; }
        public string Language { get; set; }

        public string Specification { get; set; }

        [System.Web.Mvc.AllowHtml]
        public string Description { get; set; }

        public List<Guid> PostedTagIds { get; set; }

        public XbooxLibrary.Models.DataTable.ProductTags ProductTags { get; set; }




    }
}