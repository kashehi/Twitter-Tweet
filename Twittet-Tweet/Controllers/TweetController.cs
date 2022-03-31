using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Tweetinvi;
using Twitte_Tweet.Hubs;

namespace Twittet_Tweet.Controllers
{
    public class TweetController : ApiController
    {
        private static readonly IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<HubTweetC>();
        [HttpGet]
        public async Task GetTimeLine(TwitterClient userClient)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            userClient.Config.HttpRequestTimeout = TimeSpan.FromSeconds(20);
            await userClient.RateLimits.ClearRateLimitCacheAsync();
            var homeTimeline = await userClient.Timelines.GetHomeTimelineAsync();
            await _context.Clients.All.broadcast(homeTimeline);
            //await userClient.Timelines.GetHomeTimelineAsync();
        }
    }
}
