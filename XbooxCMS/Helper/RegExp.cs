using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XbooxCMS.Helper
{
    public class RegExp
    {
        public string RemoveHtmlTag(string sentence)
        {
            var parts = System.Text.RegularExpressions.Regex.Split(sentence, @"(<p>[\s\S]+?<\/p>)").Where(l => l != string.Empty);
            var temp = String.Join("", parts);
            return temp;
        }
    }
}