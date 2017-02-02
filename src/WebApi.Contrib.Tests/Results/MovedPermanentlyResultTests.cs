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
    public class MovedPermanentlyResultTests
    {
        private const string DefaultRouteName = "routeName";
        private static readonly Uri DefaultLocation = new Uri("http://localhost/");

        [TestMethod]
        public async Task ExecuteAsyncShouldReturnAResponseWithMovedPermanentlyStatusCode()
        {
            // Arrange
            var sut = MakeSut();

            // Act
            var result = await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(HttpStatusCode.MovedPermanently, result.StatusCode);
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

        private static HttpRequestMessage MakeRequest()
        {
            var request = new HttpRequestMessage();

            return request;
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

        private static IHttpActionResult MakeSutWithController(string routeName = null, IDictionary<string, object> routeValues = null, ApiController controller = null)
        {
            var sut = new MovedPermanentlyResult
            (
                routeName ?? DefaultRouteName,
                routeValues ?? MakeRouteValues(),
                controller ?? MakeController()
            );

            return sut;
        }

        private static IHttpActionResult MakeSut(string routeName = null, IDictionary<string, object> routeValues = null, UrlHelper urlFactory = null, HttpRequestMessage request = null)
        {
            var sut = new MovedPermanentlyResult
            (
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
    }
}
