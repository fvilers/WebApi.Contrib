using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
    public class RouteLocationResultTests
    {
        private const string DefaultRouteName = "routeName";
        private static readonly Uri DefaultLocation = new Uri("http://localhost/");

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

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public async Task ExecuteAsyncShouldThrowAnExceptionForInvalidLocationFormat()
        {
            // Arrange
            var urlFactory = new Mock<UrlHelper>(MockBehavior.Strict);
            urlFactory
                .Setup(mock => mock.Link(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns<string>(null);
            var sut = MakeSut(urlFactory: urlFactory.Object);

            // Act
            await sut.ExecuteAsync(CancellationToken.None);
        }

        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithTheGeneratedLocation()
        {
            // Arrange
            var urlFactory = new Mock<UrlHelper>(MockBehavior.Strict);
            urlFactory
                .Setup(mock => mock.Link(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(DefaultLocation.ToString());
            var sut = MakeSut(urlFactory: urlFactory.Object);

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(DefaultLocation, result.Headers.Location);
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
                ),
                Url = MakeUrlFactory()
            };

            return controller;
        }

        private static IHttpActionResult MakeSutWithController(HttpStatusCode? statusCode = null, string routeName = null, IDictionary<string, object> routeValues = null, ApiController controller = null)
        {
            var sut = new TestResult(
                statusCode ?? HttpStatusCode.Ambiguous,
                routeName ?? DefaultRouteName,
                routeValues ?? MakeRouteValues(),
                controller ?? MakeController()
            );

            return sut;
        }

        private static UrlHelper MakeUrlFactory()
        {
            var urlFactory = new Mock<UrlHelper>(MockBehavior.Strict);
            urlFactory
                .Setup(mock => mock.Link(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(DefaultLocation.ToString());

            return urlFactory.Object;
        }

        private static IDictionary<string, object> MakeRouteValues()
        {
            var routeValues = new Dictionary<string, object>();

            return routeValues;
        }

        private static IHttpActionResult MakeSut(HttpStatusCode? statusCode = null, string routeName = null, IDictionary<string, object> routeValues = null, UrlHelper urlFactory = null, HttpRequestMessage request = null)
        {
            var sut = new TestResult(
                statusCode ?? HttpStatusCode.Ambiguous,
                routeName ?? DefaultRouteName,
                routeValues ?? MakeRouteValues(),
                urlFactory ?? MakeUrlFactory(),
                request ?? MakeRequest()
            );

            return sut;
        }

        private class TestController : ApiController
        {
        }

        private class TestResult : RouteLocationResult
        {
            public TestResult(HttpStatusCode statusCode, string routeName, IDictionary<string, object> routeValues, ApiController controller)
                : base(statusCode, routeName, routeValues, controller)
            {
            }

            public TestResult(HttpStatusCode statusCode, string routeName, IDictionary<string, object> routeValues, UrlHelper urlFactory, HttpRequestMessage request)
                : base(statusCode, routeName, routeValues, urlFactory, request)
            {
            }
        }
    }
}
