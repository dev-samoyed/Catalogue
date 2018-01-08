using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Catalogue.Startup))]
namespace Catalogue
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
