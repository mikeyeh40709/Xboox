﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XbooxCMS.ViewModels
{
    public class SalesRevenueViewModel
    {
        public int Month { set; get; }
        public decimal Revenue { set; get; }
    }

    public class TopProducts
    {
        public string Name { set; get; }
        public int Quantity { set; get; }
    }
}