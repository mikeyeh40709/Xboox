using ECPay.Payment.Integration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Xboox.Services;
using Xboox.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace Xboox.Models.Services
{
    public class ECPayService
    {
        public string GetSHA256(string ToLower)
        {
            SHA256 SHA256Hasher = SHA256.Create();
            byte[] data = SHA256Hasher.ComputeHash(Encoding.Default.GetBytes(ToLower));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            return sBuilder.ToString();
        }
        public SortedDictionary<string, string> GetPostCollection<T>(List<T> productItems, OrderViewModel order,string MerchantTradeNo) where T : OrderItemsBase { 
            using (var dbcontext = new XbooxLibraryDBContext())
            {
                string MerchantID = "2000214";
                string HashKey = "5294y06JbISpM5x9";
                string HashIV = "v77hoKGq4kWxNNIS";
                decimal discount = 1m; 
                if(order.Discount != null)
                {
                    var couponRepo = new GeneralRepository<Coupons>(dbcontext);
                    discount = (decimal)couponRepo.GetFirst(item => item.CouponCode == order.Discount).Discount;
                }
                
                //AllPay
                AllInOne opay = new AllInOne();
                opay.Send.MerchantTradeNo = MerchantTradeNo; //廠商訂單編號
                opay.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //廠商訂單日期           
                opay.Send.TotalAmount = Convert.ToInt32(productItems.Sum(item => item.Price * item.Quantity * discount));
                opay.Send.TradeDesc = "ECPay訂單測試";
                foreach (var item in productItems)
                {
                    opay.Send.Items.Add(new Item()
                    {
                        Name = item.Name,//商品名稱
                        Price = Convert.ToInt32(item.Price),//商品單價
                        Currency = "元",//幣別單位
                        Quantity = Convert.ToInt32(item.Quantity),//購買數量
                    });
                }
                var payment = OrderService.payment.FirstOrDefault(x => x.Key == order.Payment).Value;
                // 形成字串
                SortedDictionary<string, string> PostCollection = new SortedDictionary<string, string>();
                PostCollection.Add("MerchantID", MerchantID);
                PostCollection.Add("MerchantTradeNo", opay.Send.MerchantTradeNo);
                PostCollection.Add("MerchantTradeDate", opay.Send.MerchantTradeDate);
                PostCollection.Add("PaymentType", "aio");//固定帶aio
                PostCollection.Add("TotalAmount", Convert.ToInt32(opay.Send.TotalAmount).ToString());
                PostCollection.Add("TradeDesc", opay.Send.TradeDesc);
                PostCollection.Add("ItemName", opay.Send.ItemName);
                PostCollection.Add("ClientBackURL", "https://xboox.azurewebsites.net/Home/Index");
                PostCollection.Add("ReturnURL", "https://xboox.azurewebsites.net/Order/ECPayResult");//廠商通知付款結果API
                PostCollection.Add("ChoosePayment", payment);
                PostCollection.Add("CustomField1", order.OrderId.ToString());
                PostCollection.Add("EncryptType", "1");//固定
                //壓碼
                string str = string.Empty;
                string str_pre = string.Empty;
                foreach (var item in PostCollection)
                {
                    str += string.Format("&{0}={1}", item.Key, item.Value);
                }

                str_pre += string.Format("HashKey={0}" + str + "&HashIV={1}", HashKey, HashIV);

                string urlEncodeStrPost = HttpUtility.UrlEncode(str_pre);
                string ToLower = urlEncodeStrPost.ToLower();
                string sCheckMacValue = GetSHA256(ToLower);
                PostCollection.Add("CheckMacValue", sCheckMacValue);
                return PostCollection;
            }
        }
    }
}