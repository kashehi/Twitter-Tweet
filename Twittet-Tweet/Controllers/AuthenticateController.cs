using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Twittet_Tweet.Classes;

namespace Twittet_Tweet.Controllers
{
    public class AuthenticateController : ApiController
    {
        private static readonly IAuthenticationRequestStore _myAuthRequestStore = new LocalAuthenticationRequestStore();
        private static ITwitterClient AppClient
        {
            get
            {
                var userCredentials = TwitterApiCredentials.GetAppCredentials();
                var appCreds = new ConsumerOnlyCredentials(userCredentials.ConsumerKey, userCredentials.ConsumerSecret);
                return new TwitterClient(appCreds);
            }
        }
        //initialize the authentication process
        [HttpGet]
        public async Task<IHttpActionResult> TwitterAuth()
        {
            //To recognize display duthPage at first time
            HttpContext.Current.Session["Authenticated"] = "IsAuthenticated";
            var authenticationRequestId = Guid.NewGuid().ToString();
            var redirectPath = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port + "/api/Home/ValidateTwitterAuth";
            // Add the user identifier as a query parameters that will be received by `ValidateTwitterAuth`
            var redirectURL = _myAuthRequestStore.AppendAuthenticationRequestIdToCallbackUrl(redirectPath, authenticationRequestId);
            // Initialize the authentication process
            var authenticationRequestToken = await AppClient.Auth.RequestAuthenticationUrlAsync(redirectURL);
            // Store the token information in the store
            await _myAuthRequestStore.AddAuthenticationTokenAsync(authenticationRequestId, authenticationRequestToken);
            var myApiController = new AuthenticateController
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            HttpResponseMessage result =(myApiController.Request.CreateResponse(authenticationRequestToken.AuthorizationURL));
            var response = ResponseMessage(result);
            return response;

        }

        //a url that the user will be redirected to after having approved the application
        [HttpGet]
        public async Task<HttpResponseMessage> ValidateTwitterAuth()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var response = new HttpResponseMessage();
            var myApiController = new AuthenticateController
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
            TwitterApiCredentials.LastAuthenticatedCredentials = userCreds;
            var user = await userClient.Users.GetAuthenticatedUserAsync();
            HttpContext.Current.Session["Authenticated"] = "IsAuthenticated";
            HttpResponseMessage result = myApiController.Request.CreateResponse(user);
            var fileContents = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Forms/ValidateTwitterAuth.html"));
            response.Content = new StringContent(fileContents);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return (response);

        }
    }
}