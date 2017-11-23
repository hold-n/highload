using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BigData.Startup))]

namespace BigData
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
