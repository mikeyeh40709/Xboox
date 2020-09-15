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
    public partial class ShoppiingCartService
    {
        XbooxLibraryDBContext DbContext = new XbooxLibraryDBContext();
        private string GetCookieKey()
        {
            return HttpContext.Current.Request.Cookies["VisitorKey"].Value;
        }
        public OperationResult AddToCart()
        {
            OperationResult operationResult = new OperationResult();
            
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var CartRepo = new GeneralRepository<Cart>(DbContext);
                    var GetUserKey = GetCookieKey();
                    var cart = CartRepo.GetFirst(x => x.CartId.ToString() == GetUserKey);
                    if (cart == null)
                    {
                        cart = new Cart
                        {
                            CartId = Guid.Parse(GetUserKey),
                            UserId = HttpContext.Current.User.Identity.Name
                        };
                        CartRepo.Create(cart);
                    }
                    CartRepo.SaveContext();
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
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var GetUserKey = GetCookieKey();
                    var CartItems = JsonConvert.DeserializeObject<List<TempCartItems>>(values);
                    GeneralRepository<CartItems> CartItemsRepo = new GeneralRepository<CartItems>(DbContext);
                    GeneralRepository<Product> ProductRepo = new GeneralRepository<Product>(DbContext);
                    foreach (var item in CartItems)
                    {
                        var ProductCheck = CartItemsRepo.GetFirst(x => x.ProductId.ToString() == item.ProductId && x.CartId.ToString() == GetUserKey);
                        
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
                            CartItemsRepo.Create(cartItem);
                        }
                        else
                        {
                            ProductCheck.Quantity = item.Count;
                        }
                    }
                    CartItemsRepo.SaveContext();
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
            var GetUserKey = GetCookieKey();     
            var cartRepo = new GeneralRepository<Cart>(DbContext);
            var cartItemsRepo = new GeneralRepository<CartItems>(DbContext);
            var productRepo = new GeneralRepository<Product>(DbContext);
            var productImgRepo = new GeneralRepository<ProductImgs>(DbContext);

            using (var db = new XbooxLibraryDBContext())
            {
                List<CartViewModel> CartItemList = new List<CartViewModel>();
                var tempList = (from c in cartItemsRepo.GetAll()
                                join p in productRepo.GetAll()
                                on c.ProductId equals p.ProductId
                                join i in productImgRepo.GetAll()
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
        public OperationResult EmptyCart(string id)
        {
            OperationResult operationResult = new OperationResult();
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var CartItemsRepo = new GeneralRepository<CartItems>(DbContext);
                    var GetUserKey = GetCookieKey();
                    var cartItems = DbContext.CartItems.Where(
                    cart => cart.CartId.ToString() == GetUserKey && cart.ProductId.ToString() == id).ToList();
                    foreach (var cartItem in cartItems)
                    {
                        CartItemsRepo.Delete(cartItem);
                    }
                    CartItemsRepo.SaveContext();
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
    }
}








