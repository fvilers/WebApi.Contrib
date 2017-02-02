using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using WebApi.Contrib.Results;

namespace WebApi.Contrib.Tests.Results
{
    [TestClass]
    public class NoContentResultTests
    {
        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithNoContentStatusCode()
        {
            // Arrange
            var sut = MakeSut();

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithTheSpecifiedRequest()
        {
            // Arrange
            var request = MakeRequest();
            var sut = MakeSut(request);

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(request, result.RequestMessage);
        }

        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithTheSpecifiedControllerRequest()
        {
            // Arrange
            var request = MakeRequest();
            var sut = MakeSutWithController(MakeController(request));

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(request, result.RequestMessage);
        }

        private static ApiController MakeController(HttpRequestMessage request = null)
        {
            var controller = new TestController
            {
                ControllerContext = new HttpControllerContext
                (
                    new HttpConfiguration(),
                    new HttpRouteData(new HttpRoute()),
                    request ?? MakeRequest()
                )
            };

            return controller;
        }

        private static HttpRequestMessage MakeRequest()
        {
            var request = new HttpRequestMessage();

            return request;
        }

        private static IHttpActionResult MakeSutWithController(ApiController controller)
        {
            var sut = new NoContentResult(controller);

            return sut;
        }

        private static IHttpActionResult MakeSut(HttpRequestMessage request = null)
        {
            var sut = new NoContentResult(request ?? MakeRequest());

            return sut;
        }

        private class TestController : ApiController
        {
        }
    }
}
