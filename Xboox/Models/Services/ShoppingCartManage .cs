using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Xboox.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace Xboox.Models.Services
{
    public partial class ShoppingCartManage
    {
        XbooxLibraryDBContext DbContext = new XbooxLibraryDBContext();
        private string GetCookieKey()
        {
            return HttpContext.Current.Request.Cookies["VisitorKey"].Value;
        }
        public OperationResult AddToCart()
        {
            OperationResult operationResult = new OperationResult();
            GeneralRepository<Cart> CartRepository = new GeneralRepository<Cart>(DbContext);
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var GetUserKey = GetCookieKey();
                    var cart = DbContext.Cart.FirstOrDefault(ca => ca.CartId.ToString() == GetUserKey);
                    if (cart == null)
                    {
                        cart = new Cart
                        {
                            CartId = Guid.Parse(GetUserKey),
                            UserId = HttpContext.Current.User.Identity.Name
                        };
                        CartRepository.Create(cart);
                    }
                    DbContext.SaveChanges();
                    operationResult.isSuccessful = true;
                    transaction.Commit();
                    return operationResult;
                }
                catch (Exception ex)
                {
                    operationResult.isSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                    return operationResult;

                }

            }

        }

        public OperationResult AddToCartItems(string values)
        {
            OperationResult operationResult = new OperationResult();
            GeneralRepository<CartItems> CartItemsRepository = new GeneralRepository<CartItems>(DbContext);
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var GetUserKey = GetCookieKey();
                    var CartItems = JsonConvert.DeserializeObject<List<TempCartItems>>(values);
                    foreach (var item in CartItems)
                    {
                        var ProductCheck = DbContext.CartItems.FirstOrDefault(
                       c => c.ProductId.ToString() == item.ProductId && c.CartId.ToString() == GetUserKey);
                        if (ProductCheck == null)
                        {
                            Guid randomId = Guid.NewGuid();
                            CartItems cartItem = new CartItems
                            {
                                CartId = Guid.Parse(GetUserKey),
                                ProductId = Guid.Parse(item.ProductId),
                                Quantity = item.Count,
                                Id = randomId
                            };
                            CartItemsRepository.Create(cartItem);
                        }
                        else
                        {
                            ProductCheck.Quantity = item.Count;
                        }
                    }
                    DbContext.SaveChanges();
                    operationResult.isSuccessful = true;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    operationResult.isSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                }
            }
            return operationResult;
        }


        public List<CartViewModel> GetCartItems(HttpContextBase context)
        {
            var watch = new Stopwatch();
            watch.Start();
            var GetUserKey = context.Request.Cookies["VisitorKey"].Value;
            using (var db = new XbooxLibraryDBContext())
            {
                List<CartViewModel> CartItemList = new List<CartViewModel>();
                var tempList = (from c in db.CartItems
                                join p in db.Product
                                on c.ProductId equals p.ProductId
                                join i in db.ProductImgs
                                on p.ProductId equals i.ProductId
                                where p.ProductId == i.ProductId
                                where c.CartId.ToString() == GetUserKey
                                select new CartViewModel
                                {
                                    ProductId = p.ProductId,
                                    ProductImgLink = i.imgLink,
                                    Name = p.Name,
                                    Price = p.Price,
                                    Quantity = c.Quantity,
                                    TotalPrice = c.Quantity * p.Price

                                }).GroupBy(item => item.Name);
                foreach (var pdList in tempList)
                {

                    var firstItem = pdList.FirstOrDefault(item => !item.Name.Contains("-0"));
                    CartItemList.Add(firstItem);
                }
                watch.Stop();
                Debug.WriteLine(watch.ElapsedMilliseconds);
                return CartItemList;
            }

        }

    }
}








