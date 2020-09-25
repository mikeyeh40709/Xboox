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
        /// <summary>
        /// 取得Taipei 時間
        /// </summary>
        /// <param name="orderDate"></param>
        /// <returns>回傳台北時間沒有則為null</returns>
        public static DateTime? GetTaipeiTime(DateTime? orderDate)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime cstTime = new DateTime();
            if(orderDate != null)
            {
                cstTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)orderDate, cstZone);
                return cstTime;
            }
            return null;
        }
    }
}