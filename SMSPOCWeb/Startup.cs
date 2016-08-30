using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMSPOCWeb.Startup))]
namespace SMSPOCWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
