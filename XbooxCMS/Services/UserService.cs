using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;
using XbooxCMS.ViewModels;

namespace XbooxCMS.Services
{
    public class UserService
    {

        public List<UserListViewModel> GetAllUsers()
        {

           XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            var userrepo = new GeneralRepository<AspNetUsers>(context);
            var userList = userrepo.GetAll().ToList();
            List<UserListViewModel> viewModel = new List<UserListViewModel>();
            foreach(var item in userList)
            {
                viewModel.Add(new UserListViewModel()
                {
                    Id = item.Id,
                    Email = item.Email,
                    UserName = item.UserName,
                    PhoneNumber = item.PhoneNumber
                });
            }

            return viewModel;
        }
    }
}