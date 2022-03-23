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

namespace Twitte_Tweet.Hubs
{
    [HubName("hubTweet")]
    public class HubTweetC : Hub
    {
        public async Task ConnectToTwitter()
        {
            var userClient = new TwitterClient("054AHr7Ncjdytrj7YTRWtHCni", "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl", "1504021761372745731-bh9u5C5LiMzdtIbsq3YKOFvmZUwEY1", "xNP1p4ZapWbbLuxHvj38BzvW70gcC44daOPyqGtJzPiwb");
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var sampleStream = userClient.Streams.CreateSampleStream();
            sampleStream.TweetReceived += (sender, eventArgs) =>
            {
                Clients.All.broadcast(eventArgs.Tweet.FullText + ";" + eventArgs.Tweet.CreatedAt + ";" + eventArgs.Tweet.CreatedBy);
            };

            await sampleStream.StartAsync();
        }
    }
}