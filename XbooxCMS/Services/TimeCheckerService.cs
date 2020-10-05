using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XbooxCMS.Services
{
    public class TimeCheckerService
    {
        public static DateTime? GetTaipeiTime(DateTime? orderDate)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime cstTime = new DateTime();
            if (orderDate != null)
            {
                cstTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)orderDate, cstZone);
                return cstTime;
            }
            return null;
        }
    }
}