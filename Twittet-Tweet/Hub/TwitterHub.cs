using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Tweetinvi;
using Twittet_Tweet.Classes;
using System.Threading;


namespace Twitte_Tweet.Hubs
{
    [HubName("hubTweet")]
    public class HubTweetC : Hub
    {
        //User credentials info and Api
        private static Tweetinvi.Models.ITwitterCredentials authenticateduser = TwitterApiCredentials.LastAuthenticatedCredentials;
        private Tweetinvi.Models.IAuthenticatedUser profileInformation = ProfileInfo.ProfileInformation;
        private static Tweetinvi.Models.IConsumerOnlyCredentials userCredentials = TwitterApiCredentials.GetAppCredentials();
        private TwitterClient userClient = new TwitterClient(userCredentials.ConsumerKey, userCredentials.ConsumerSecret, authenticateduser.AccessToken, authenticateduser.AccessTokenSecret);

        //Get Tweest in real-time
        public async Task GetTweets()
        {

            var sampleStream = userClient.Streams.CreateSampleStream();

            sampleStream.TweetReceived += (sender, eventArgs) =>
            {
                Clients.All.broadcast(eventArgs.Tweet);
                //Delay for show tweets
                Thread.Sleep(5000);

            };

            await sampleStream.StartAsync();

        }
        // Get user info who login
        public void GetUserInfo()
        {
            Clients.All.ProfileInfo(profileInformation);
        }
        // Get trend location
        public async Task Gettrending()
        {
            var trendingLocations = await userClient.Trends.GetTrendLocationsAsync();
            Clients.All.trending(trendingLocations);

        }

    }
}