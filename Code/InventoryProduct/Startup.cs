using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InventoryProduct.Startup))]
namespace InventoryProduct
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
