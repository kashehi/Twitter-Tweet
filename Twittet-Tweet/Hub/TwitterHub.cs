using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Tweetinvi;
using System.Net;
using Tweetinvi.Models;
using System.Threading;
using Tweetinvi.Streams;
using Twittet_Tweet.Classes;

namespace Twitte_Tweet.Hubs
{
    [HubName("hubTweet")]
    public class HubTweetC : Hub
    {
       
        public async Task GetTweets()
        {
            var userClient = new TwitterClient("054AHr7Ncjdytrj7YTRWtHCni", "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl", "1504021761372745731-bh9u5C5LiMzdtIbsq3YKOFvmZUwEY1", "xNP1p4ZapWbbLuxHvj38BzvW70gcC44daOPyqGtJzPiwb");
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            userClient.Config.HttpRequestTimeout = TimeSpan.FromSeconds(20);
            await userClient.RateLimits.ClearRateLimitCacheAsync();
            var homeTimeline = await userClient.Timelines.GetHomeTimelineAsync();
            Clients.All.broadcast(homeTimeline);
            await userClient.Timelines.GetHomeTimelineAsync();
        }
    }
}