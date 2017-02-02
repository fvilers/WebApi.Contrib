using System.Web.Http;
using WebApi.Contrib.Results;

namespace WebApi.Contrib
{
    public abstract class ExtendedApiController : ApiController
    {
        public NoContentResult NoContent()
        {
            return new NoContentResult(this);
        }
    }
}
