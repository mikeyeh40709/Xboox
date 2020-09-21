using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XbooxCMS.Services
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