using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.ViewModels;
using XbooxLibrary.Models.DataTable;

namespace Xboox.Models.Services
{
    public class TimeCheckerService
    {
        public DateTime GetTaipeiTime(DateTime orderDate)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(orderDate, cstZone);
            return cstTime;
        }
    }
}