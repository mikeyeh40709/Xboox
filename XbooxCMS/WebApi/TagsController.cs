using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XbooxCMS.Services;
using XbooxCMS.ViewModels;
using XbooxLibrary.Services;

namespace XbooxCMS.WebApi
{
    [RoutePrefix("api/[Controller]/[Action]")]
    public class TagsController : ApiController
    {
        [HttpGet]
        public List<TagViewModel> GetTags()
        {
           TagService tagService = new TagService();
            var tagList = tagService.GetTags();
            return tagList;
        }

    }
}
