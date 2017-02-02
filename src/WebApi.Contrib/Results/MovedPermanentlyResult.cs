using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApi.Contrib.Results
{
    public class MovedPermanentlyResult : RouteLocationResult
    {
        public MovedPermanentlyResult(string routeName, IDictionary<string, object> routeValues, ApiController controller)
            : base(HttpStatusCode.MovedPermanently, routeName, routeValues, controller)
        {
        }

        public MovedPermanentlyResult(string routeName, IDictionary<string, object> routeValues, UrlHelper urlFactory, HttpRequestMessage request)
            : base(HttpStatusCode.MovedPermanently, routeName, routeValues, urlFactory, request)
        {
        }
    }
}
