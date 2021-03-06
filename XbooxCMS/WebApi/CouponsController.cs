﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XbooxCMS.Services;
using XbooxLibrary.Models.DataTable;

namespace XbooxCMS.WebApi
{
    [RoutePrefix("api/[Controller]/[Action]")]
    public class CouponsController : ApiController
    {
        //[Route("GetCoupons")]
        [HttpGet]
        public List<Coupons> GetCoupons()
        {
            CouponsService service = new CouponsService();
            return service.GetAllCoupons();
        }
        //[Route("SaveEditCoupon")]
        [HttpPut]
        public IHttpActionResult SaveEditCoupon([FromBody]Coupons data)
        {
            CouponsService service = new CouponsService();
            service.CouponsEdit(data);
            return Ok(data);
        }
        //[Route("CreateCoupon")]
        [HttpPost]
        public IHttpActionResult CreateCoupon([FromBody]Coupons data)
        {
            CouponsService service = new CouponsService();
            service.CouponsCreate(data);
            return Ok(data);
        }
        [HttpDelete]
        public IHttpActionResult DeleteCoupon([FromBody]Guid id)
        {
            CouponsService service = new CouponsService();
            service.DeleteConfirmed(id);
            return Ok(id);
        }

    }


    //[RoutePrefix("api/[Controller]/[Action]")]
    //public class CouponsCreateController : ApiController
    //{
    //    [HttpPost]
    //    public IHttpActionResult CreateCoupon([FromBody]Coupons data)
    //    {
    //        CouponsService service = new CouponsService();
    //        service.CouponsCreate(data);
    //        return Ok(data);
    //    }


    //    [HttpPut]
    //    public IHttpActionResult DeleteCoupon([FromBody]Guid id)
    //    {
    //        CouponsService service = new CouponsService();
    //        service.DeleteConfirmed(id);
    //        return Ok(id);
    //    }
    //}


}
