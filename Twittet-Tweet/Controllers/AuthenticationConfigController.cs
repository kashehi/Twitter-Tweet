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
using Tweetinvi.Parameters;

namespace Twittet_Tweet.Controllers
{
    public class AuthenticationConfigController : ApiController
    {
        private static readonly IAuthenticationRequestStore _myAuthRequestStore = new LocalAuthenticationRequestStore();
        [HttpGet]
        public async Task<IHttpActionResult> TwitterAuth()
        {

            var appClient = new TwitterClient("054AHr7Ncjdytrj7YTRWtHCni", "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl");
            var authenticationRequestId = Guid.NewGuid().ToString();
            var redirectPath = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host+":"+ Request.RequestUri.Port + "/api/AuthenticationConfig/ValidateTwitterAuth";

            // Add the user identifier as a query parameters that will be received by `ValidateTwitterAuth`
            var redirectURL = _myAuthRequestStore.AppendAuthenticationRequestIdToCallbackUrl(redirectPath, authenticationRequestId);
            // Initialize the authentication process
            var authenticationRequestToken = await appClient.Auth.RequestAuthenticationUrlAsync(redirectURL);
            // Store the token information in the store
            await _myAuthRequestStore.AddAuthenticationTokenAsync(authenticationRequestId, authenticationRequestToken);

            var myApiController = new AuthenticationConfigController
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            HttpResponseMessage result = myApiController.Request.CreateResponse(authenticationRequestToken.AuthorizationURL);
            var response = ResponseMessage(result);
            return response;



        }
        [HttpGet]
        public async Task<IHttpActionResult> ValidateTwitterAuth()
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var appClient = new TwitterClient("054AHr7Ncjdytrj7YTRWtHCni", "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl");
            var myApiController = new AuthenticationConfigController
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
                
                // Extract the information from the redirection url
                var requestParameters = await RequestCredentialsParameters.FromCallbackUrlAsync(Request.RequestUri.Query.ToString(), _myAuthRequestStore);
            // Request Twitter to generate the credentials.
            var userCreds = await appClient.Auth.RequestCredentialsAsync(requestParameters);

            // Congratulations the user is now authenticated!
            var userClient = new TwitterClient(userCreds);
            var user = await userClient.Users.GetAuthenticatedUserAsync();

            HttpResponseMessage result = myApiController.Request.CreateResponse(user);
            var response = ResponseMessage(result);
            return response;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


    }
}