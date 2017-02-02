using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using WebApi.Contrib.Results;

namespace WebApi.Contrib
{
    public abstract class ExtendedApiController : ApiController
    {
        public NoContentResult NoContent()
        {
            return new NoContentResult(this);
        }

        public MovedPermanentlyResult RedirectPermanentlyToRoute(string routeName, object routeValues)
        {
            return RedirectPermanentlyToRoute(routeName, new HttpRouteValueDictionary(routeValues));
        }

        public MovedPermanentlyResult RedirectPermanentlyToRoute(string routeName, IDictionary<string, object> routeValues)
        {
            return new MovedPermanentlyResult(routeName, routeValues, this);
        }
    }
}
