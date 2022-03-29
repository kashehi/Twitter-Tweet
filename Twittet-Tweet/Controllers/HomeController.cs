using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Net.Http.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Twitte_Tweet.Hubs;
using Twittet_Tweet.Classes;

namespace Twittet_Tweet.Controllers
{
    public class HomeController : ApiController
    {
        private static readonly IAuthenticationRequestStore _myAuthRequestStore = new LocalAuthenticationRequestStore();
        private static ITwitterClient AppClient
        {
            get
            {
                var userCredentials = Credentials.GetAppCredentials();
                var appCreds = new ConsumerOnlyCredentials(userCredentials.ConsumerKey, userCredentials.ConsumerSecret);
                return new TwitterClient(appCreds);
            }
        }
        //initialize the authentication process
        [HttpGet]
        public async Task<IHttpActionResult> TwitterAuth()
        {
            var authenticationRequestId = Guid.NewGuid().ToString();

            var redirectPath = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port + "/api/Home/ValidateTwitterAuth";

            // Add the user identifier as a query parameters that will be received by `ValidateTwitterAuth`
            var redirectURL = _myAuthRequestStore.AppendAuthenticationRequestIdToCallbackUrl(redirectPath, authenticationRequestId);

            // Initialize the authentication process
            var authenticationRequestToken = await AppClient.Auth.RequestAuthenticationUrlAsync(redirectURL);

            // Store the token information in the store
            await _myAuthRequestStore.AddAuthenticationTokenAsync(authenticationRequestId, authenticationRequestToken);

            var myApiController = new HomeController
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            HttpResponseMessage result = myApiController.Request.CreateResponse(authenticationRequestToken.AuthorizationURL);
            var response = ResponseMessage(result);
            return response;

        }
        //a url that the user will be redirected to after having approved the application
        [HttpGet]
        public async Task<IHttpActionResult> ValidateTwitterAuth()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var myApiController = new HomeController
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Extract the information from the redirection url
            var requestParameters = await RequestCredentialsParameters.FromCallbackUrlAsync(Request.RequestUri.Query.ToString(), _myAuthRequestStore);

            // Request Twitter to generate the credentials.
            var userCreds = await AppClient.Auth.RequestCredentialsAsync(requestParameters);

            //  the user is now authenticated!
            var userClient = new TwitterClient(userCreds);

            var user = await userClient.Users.GetAuthenticatedUserAsync();

            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<HubTweetC>();
            //var homeTimeline = await userClient.Timelines.GetHomeTimelineAsync();
            //hubContext.Clients.All.broadcast(homeTimeline);
            //await userClient.Timelines.GetHomeTimelineAsync();

            HttpResponseMessage result = myApiController.Request.CreateResponse(user);
            var response = ResponseMessage(result);
            return response;

        }
    }
}