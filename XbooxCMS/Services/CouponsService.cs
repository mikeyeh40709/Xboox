using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;
using XbooxLibrary.Services;

namespace XbooxCMS.Services
{
    public class CouponsService
    {
        public List<Coupons> GetAllCoupons()
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Coupons> couponRepo = new GeneralRepository<Coupons>(context);
            var couponList = couponRepo.GetAll().ToList();
            return couponList;
        }

        public OperationResult CouponsEdit(Coupons input)
        {
            var result = new OperationResult();
            try
            {
                XbooxLibraryDBContext context = new XbooxLibraryDBContext();
                GeneralRepository<Coupons> couRepo = new GeneralRepository<Coupons>(context);

                couRepo.Update(input);
                couRepo.SaveContext();
                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;
        }


        public OperationResult CouponsCreate(Coupons input)
        {
            var result = new OperationResult();
            try
            {
                XbooxLibraryDBContext context = new XbooxLibraryDBContext();
                GeneralRepository<Coupons> couRepo = new GeneralRepository<Coupons>(context);
                Coupons entity = new Coupons()
                {
                    Id = Guid.NewGuid(),
                    CouponName = input.CouponName,
                    Discount = input.Discount,
                    CouponCode = input.CouponCode,
                    StartTime = input.StartTime,
                    EndTime = input.EndTime
                };
                couRepo.Create(entity);
                couRepo.SaveContext();
                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;
        }

        public OperationResult DeleteConfirmed(Guid id)
        {
            var result = new OperationResult();
            try
            {
                XbooxLibraryDBContext context = new XbooxLibraryDBContext();
                GeneralRepository<Coupons> couRepo = new GeneralRepository<Coupons>(context);

              
                Coupons coupons = context.Coupons.Find(id);
                context.Coupons.Remove(coupons);
                context.SaveChanges();
                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;

        }




    }
}