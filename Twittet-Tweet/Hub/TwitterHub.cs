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
using Tweetinvi.Client;

namespace Twitte_Tweet.Hubs
{
    [HubName("hubTweet")]
    public class HubTweetC : Hub<Twittet_Tweet.Classes.ITweet>
    {
        public async Task GetTweets(ITimelinesClient Timeline)
        {
            await Clients.All.broadcast(Timeline);

        }
    }
}