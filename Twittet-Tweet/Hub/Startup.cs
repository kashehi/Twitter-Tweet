using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Twittet_Tweet.Hub.Startup))]

namespace Twittet_Tweet.Hub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           //Connect hub To tunel
            app.MapSignalR();

        }
    }
}
