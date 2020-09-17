using System;
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
        public int Month { set; get; }
        public string Name { set; get; }
        public int Quantity { set; get; }
    }

    public class GetTitleData
    {
        public int products { set; get; }
        public int members { set; get; }
        public int revenue { set; get; }
        public int orders { set; get; }
    }

}