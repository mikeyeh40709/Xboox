using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XbooxCMS.Services;
using XbooxCMS.ViewModels;

namespace XbooxCMS.WebApi
{
    [RoutePrefix("api/[Controller]/[Action]")]

    public class UserController : ApiController
    {
        [HttpGet]
        public List<UserListViewModel> ListUser()
        {
            UserService service = new UserService();
            var userList = service.GetAllUsers();
            return userList;
        }
    }
}
