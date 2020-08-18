using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Xboox.Startup))]
namespace Xboox
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
