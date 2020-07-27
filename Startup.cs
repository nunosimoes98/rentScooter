using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eRecarga.Startup))]
namespace eRecarga
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
