using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SklepInternetowy.Startup))]
namespace SklepInternetowy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
