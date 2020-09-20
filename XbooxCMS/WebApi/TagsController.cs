using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using XbooxCMS.Services;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;

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
        [HttpPost]
        public IHttpActionResult SaveEditTag([FromBody]Tags id)
        {
            TagService service = new TagService();
            service.Edit(id);
            return Ok(id);
        }
    }
    [RoutePrefix("api/[Controller]/[Action]")]

    public class TagsCreateController : ApiController
    {
        [HttpPost]
        public string CreateTags([FromBody]Tags name)
        {
            TagService service = new TagService();
            service.Create(name);
            return "ABC";
        }
    }
}
