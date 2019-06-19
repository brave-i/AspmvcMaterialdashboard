using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chatison.Startup))]
namespace Chatison
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
