using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shauli.Startup))]
namespace Shauli
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
