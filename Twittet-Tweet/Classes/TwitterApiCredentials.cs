using Tweetinvi.Models;

namespace Twittet_Tweet.Classes
{
    public static class TwitterApiCredentials
    {
        //Set API Key and API Key Secret of twitter api
        public static IConsumerOnlyCredentials GetAppCredentials()
        {
            return new ConsumerOnlyCredentials("054AHr7Ncjdytrj7YTRWtHCni", "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl");
        }
        
        public static ITwitterCredentials LastAuthenticatedCredentials { get; set; }

    }
}