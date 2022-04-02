using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Tweetinvi;
using System.Net;
using Twittet_Tweet.Classes;
using System;
using System.Threading;

namespace Twitte_Tweet.Hubs
{
    [HubName("hubTweet")]
    public class HubTweetC : Hub
    {
        public async Task GetTweets()
        {
            var authenticateduser = TwitterApiCredentials.LastAuthenticatedCredentials;
            var userCredentials = TwitterApiCredentials.GetAppCredentials();
            var userClient = new TwitterClient(userCredentials.ConsumerKey, userCredentials.ConsumerSecret, authenticateduser.AccessToken, authenticateduser.AccessTokenSecret);
            var sampleStream = userClient.Streams.CreateSampleStream();
            sampleStream.TweetReceived += (sender, eventArgs) =>
            {
                Clients.All.broadcast(eventArgs.Tweet);
                //Delay for show tweets
                Thread.Sleep(80000);
            };
            await sampleStream.StartAsync();

        }
    }
}