using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XbooxLibrary.Services
{
    public class OperationResult
    {
        public bool IsSuccessful { get; set; }

        public Exception exception { get; set; }
    }

    public static class OperationResultHelper
    {
        public static string WriteLog(this OperationResult value)
        {
            if (value.exception != null)
            {
                Debug.WriteLine($"{ value.exception}");
                //string path = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
               // path = path + ".txt";
               // File.WriteAllText(path, value.exception.ToString());
                return value.exception.ToString();
            }
            else
            {
                return "OK";
            }

        }
    }
}
