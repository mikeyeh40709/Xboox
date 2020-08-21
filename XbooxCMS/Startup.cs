using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XbooxCMS.Startup))]
namespace XbooxCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
