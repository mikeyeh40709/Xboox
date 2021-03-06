﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class SalesRevenueViewModel
    {
        public int Year { set; get; }
        public int Month { set; get; }
        public decimal Revenue { set; get; }
    }

    public class TopProducts
    {
        public int Year { set; get; }
        public int Month { set; get; }
        public string Name { set; get; }
        public int Quantity { set; get; }
    }

    public class TitleData
    {
        public int products { set; get; }
        public int members { set; get; }
        public decimal revenue { set; get; }
        public int orders { set; get; }
    }

}