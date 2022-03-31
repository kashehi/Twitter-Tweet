using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client;

namespace Twittet_Tweet.Classes
{
    public interface ITweet
    {
        Task broadcast(ITimelinesClient Timeline);
    }
}
