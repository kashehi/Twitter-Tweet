using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Twittet_Tweet.Hub
{
    public class TwitterHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}