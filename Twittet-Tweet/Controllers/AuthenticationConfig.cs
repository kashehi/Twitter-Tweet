using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Net.Http.Server;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AuthenticationConfig : ApiController
    {

        public static string GetAuthenticationConfig()
        {
            var APIKey = "054AHr7Ncjdytrj7YTRWtHCni";
            var APIKeySecret = "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl";
            var BearerToken = "AAAAAAAAAAAAAAAAAAAAAAqAaQEAAAAAoL1BIi % 2BB % 2FHwnEk4X9jxqMUSzK9U % 3DMxVKq5nCOAfMJ7R8Sq0DNAOS8z94OYy5bqjMbbVYMsjJ6PbFaw";
            var AccessToken = "1504021761372745731-bh9u5C5LiMzdtIbsq3YKOFvmZUwEY1";
            var AccessTokenSecret = " xNP1p4ZapWbbLuxHvj38BzvW70gcC44daOPyqGtJzPiwb";
            return "";
        }
        private static readonly IAuthenticationRequestStore _myAuthRequestStore = new LocalAuthenticationRequestStore();

       
        public async Task<IHttpActionResult>  TwitterAuth()
        {
            try
            {
                
                var appClient = new TwitterClient("054AHr7Ncjdytrj7YTRWtHCni", "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl");
                var authenticationRequestId = Guid.NewGuid().ToString();
                var redirectPath ="http" + "://" + "192.168.1.103:60580" + "/Home/ValidateTwitterAuth";

                // Add the user identifier as a query parameters that will be received by `ValidateTwitterAuth`
                var redirectURL = _myAuthRequestStore.AppendAuthenticationRequestIdToCallbackUrl(redirectPath, authenticationRequestId);
                // Initialize the authentication process
                var authenticationRequestToken = await appClient.Auth.RequestAuthenticationUrlAsync(redirectURL);
                // Store the token information in the store
                await _myAuthRequestStore.AddAuthenticationTokenAsync(authenticationRequestId, authenticationRequestToken);

                var myApiController = new AuthenticationConfig
                {
                    Request = new System.Net.Http.HttpRequestMessage(),
                    Configuration = new HttpConfiguration()
                };

                HttpResponseMessage result = myApiController.Request.CreateResponse(authenticationRequestToken.AuthorizationURL);
                var response = ResponseMessage(result);
                return response;

                //return   Request.CreateResponse(authenticationRequestToken.AuthorizationURL);
                // Redirect the user to Twitter
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //public async Task<IHttpActionResult> ValidateTwitterAuth()
        //{
        //    try
        //    {

           
        //    var appClient = new TwitterClient("054AHr7Ncjdytrj7YTRWtHCni", "XV7YDUIkxi7Z290TtHZxAu8DqxsUMFSmW6arzcnfIeBW2ldxPl");
        //        var myApiController = new AuthenticationConfig
        //        {
        //            Request = new System.Net.Http.HttpRequestMessage(),
        //            Configuration = new HttpConfiguration()
        //        };

        //        // Extract the information from the redirection url
        //        var requestParameters = await RequestCredentialsParameters.FromCallbackUrlAsync("http://192.168.1.103:60580/Home/ValidateTwitterAuth", _myAuthRequestStore);
        //    // Request Twitter to generate the credentials.
        //    var userCreds = await appClient.Auth.RequestCredentialsAsync(requestParameters);

        //    // Congratulations the user is now authenticated!
        //    var userClient = new TwitterClient(userCreds);
        //    var user = await userClient.Users.GetAuthenticatedUserAsync();

            
        //    HttpResponseMessage result = myApiController.Request.CreateResponse(user);
        //    var response = ResponseMessage(result);
        //    return response;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}


    }
}