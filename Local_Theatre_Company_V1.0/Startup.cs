using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Local_Theatre_Company_V1._0.Startup))]
namespace Local_Theatre_Company_V1._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
