using System.Web;
using System.Web.Optimization;

namespace Xboox
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/JavaScript/Origin/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/JavaScript/Origin/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Assets/JavaScript/Origin/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/JavaScript/Origin/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/templateJS").Include(
                      "~/Assets/TemplateJavaScript/jquery-3.3.1.min.js",
                      "~/Assets/TemplateJavaScript/bootstrap.min.js",
                      "~/Assets/TemplateJavaScript/jquery.magnific-popup.min.js",
                      "~/Assets/TemplateJavaScript/jquery-ui.min.js",
                      "~/Assets/TemplateJavaScript/mixitup.min.js",
                      "~/Assets/TemplateJavaScript/jquery.countdown.min.js",
                      "~/Assets/TemplateJavaScript/jquery.slicknav.js",
                      "~/Assets/TemplateJavaScript/owl.carousel.min.js",
                      "~/Assets/TemplateJavaScript/jquery.nicescroll.min.js",
                      "~/Assets/TemplateJavaScript/main.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/CustomJS").Include(
                      "~/Assets/JavaScript/Custom/HomePage/HomePage.js",
                      "~/Assets/JavaScript/Custom/CartPage/shop-cart.js",
                      "~/Assets/JavaScript/Custom/CartPage/set_to_local_storage.js"
                ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/CSS/Origin/Content/bootstrap.css",
                      "~/Assets/CSS/Origin/Content/site.css"));
            bundles.Add(new StyleBundle("~/Ashion/css").Include(
                      "~/Assets/CSS/Custom/Common/Ashion.css"));
            bundles.Add(new StyleBundle("~/bundles/templateCSS").Include(
                      "~/Assets/TemplateCSS/bootstrap.min.css",
                      "~/Assets/TemplateCSS/jquery-ui.min.css",
                      "~/Assets/TemplateCSS/magnific-popup.css",
                      "~/Assets/TemplateCSS/owl.carousel.min.css",
                      "~/Assets/TemplateCSS/slicknav.min.css",
                      "~/Assets/TemplateCSS/style.css",
                      "~/Assets/TemplateCSS/elegant-icons.css",
                      "~/Assets/TemplateCSS/font-awesome.min.css"
                ));
            bundles.Add(new StyleBundle("~/bundles/CustomCSS").Include(
                      "~/Assets/CSS/Custom/HomePage/HomePage.css"
                ));
        }
    }
}
