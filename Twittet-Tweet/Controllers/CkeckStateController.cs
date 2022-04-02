using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Twittet_Tweet.Controllers
{
    public class CkeckStateController : ApiController
    {
        [HttpGet]
        public string CheckStateUser()
        {
            var session = HttpContext.Current.Session;
            if (session["Authenticated"] != null)
            {
                var authenticated = session["Authenticated"].ToString();
                return authenticated;
            }
            else
            {
                return "";
            }

        }
    }
}
