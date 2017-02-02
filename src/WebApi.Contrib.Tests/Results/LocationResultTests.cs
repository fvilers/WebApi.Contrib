using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
    public class LocationResultTests
    {
        private const string DefaultLocation = "http://localhost/";

        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithTheSpecifiedStatusCode()
        {
            // Arrange
            const HttpStatusCode statusCode = HttpStatusCode.TemporaryRedirect;
            var sut = MakeSut(statusCode);

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(statusCode, result.StatusCode);
        }

        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithTheSpecifiedRequest()
        {
            // Arrange
            var request = MakeRequest();
            var sut = MakeSut(request: request);

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
            var sut = MakeSutWithController(controller: MakeController(request));

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(request, result.RequestMessage);
        }

        [ExpectedException(typeof(UriFormatException))]
        [TestMethod]
        public async Task ExecuteAsyncShouldThrowAnExceptionForInvalidLocationFormat()
        {
            // Arrange
            var sut = MakeSut(location: "invalid format");

            // Act
            await sut.ExecuteAsync(CancellationToken.None);
        }

        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithTheSpecifiedLocation()
        {
            // Arrange
            var location = new Uri("http://localhost/somewhere");
            var sut = MakeSut(location: location.ToString());

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(location, result.Headers.Location);
        }

        private static HttpRequestMessage MakeRequest()
        {
            var request = new HttpRequestMessage();

            return request;
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

        private static IHttpActionResult MakeSutWithController(HttpStatusCode? statusCode = null, string location = null, ApiController controller = null)
        {
            var sut = new TestResult(
                statusCode ?? HttpStatusCode.Ambiguous,
                location ?? DefaultLocation,
                controller ?? MakeController()
            );

            return sut;
        }

        private static IHttpActionResult MakeSut(HttpStatusCode? statusCode = null, string location = null, HttpRequestMessage request = null)
        {
            var sut = new TestResult(
                statusCode ?? HttpStatusCode.Ambiguous,
                location ?? DefaultLocation,
                request ?? MakeRequest()
            );

            return sut;
        }

        private class TestController : ApiController
        {
        }

        private class TestResult : LocationResult
        {
            public TestResult(HttpStatusCode statusCode, string location, ApiController controller)
                : base(statusCode, location, controller)
            {
            }

            public TestResult(HttpStatusCode statusCode, string location, HttpRequestMessage request)
                : base(statusCode, location, request)
            {
            }
        }
    }
}
