using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlowerShop.Startup))]
namespace FlowerShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
