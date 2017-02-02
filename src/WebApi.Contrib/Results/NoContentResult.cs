using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebApi.Contrib.Results
{
    public class NoContentResult : StatusCodeResult
    {
        public NoContentResult(HttpRequestMessage request)
            : base(HttpStatusCode.NoContent, request)
        {
        }

        public NoContentResult(ApiController controller)
            : base(HttpStatusCode.NoContent, controller)
        {
        }
    }
}
