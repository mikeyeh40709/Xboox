using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace Xboox.Helper
{
    public class NavBarHelper
    {
        GeneralRepository<Category> SideBarCategories;
        public NavBarHelper()
        {
            if (SideBarCategories == null)
            {
                XbooxLibraryDBContext db = new XbooxLibraryDBContext();
                SideBarCategories = new GeneralRepository<Category>(db);
            }
        }
        /// <summary>
        /// 動態生成NavbarCateogries
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetNavBarCategories()
        {
            var categories = SideBarCategories.GetAll().Select(x => x.Name).OrderBy(y => y.Substring(0, 1)).ToList();
            return categories;
        }
    }
}