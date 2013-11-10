using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wtfAuth2.Startup))]
namespace wtfAuth2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
