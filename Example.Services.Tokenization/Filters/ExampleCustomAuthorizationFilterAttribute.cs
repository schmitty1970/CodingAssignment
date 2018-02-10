using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Example.Services.Tokenization.Filters
{
    /// <summary>
    /// EXAMPLE ONLY - this is an example of an authorization filter to authenticate 'known' client applications.
    /// The point is that a REST API like this needs to be secured in some manner.
    /// This is not production ready and is only a sample for this coding exercise.
    /// 
    /// In production, one could use an OAuth2/OpenID Connect service (ex. Thinktechture's Identity Server) to implement
    /// server-to-server authentication.
    /// </summary>
    /// 
    /// Test Using Fiddler posting to http://localhost:51211/api/store/ with some text in the body and the following headers
    /// User-Agent: Fiddler
    //  Host: localhost:51211
    //  Content-Type: application/json
    //  Authorization: WeakCustom Fiddler


    public class ExampleCustomAuthorizationFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null)
            {
                var tokens = ConfigurationManager.AppSettings["KnownClientTokens"];

                if (string.IsNullOrWhiteSpace(tokens))
                    return false;

                var tokenList = tokens.Split(new char[] {','});

                if (auth.Scheme != "WeakCustom")
                    return false;

                var param = auth.Parameter;
                if (!tokenList.Contains(param.Trim()))
                    return false;

                return true;

            }

            //default secure, deny access
            return false;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {            
            //default is to return HTTP 401 - unauthorized
            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}