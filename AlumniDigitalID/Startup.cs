using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlumniDigitalID.Startup))]

namespace AlumniDigitalID
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}