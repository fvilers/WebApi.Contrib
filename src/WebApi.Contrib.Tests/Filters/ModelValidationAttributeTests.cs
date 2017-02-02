using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using WebApi.Contrib.Filters;

namespace WebApi.Contrib.Tests.Filters
{
    [TestClass]
    public class ModelValidationAttributeTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void OnActionExecutingShouldThrowAnExceptionForNullActionContext()
        {
            // Arrange
            var sut = MakeSut();

            // Act
            sut.OnActionExecuting(null);
        }

        [TestMethod]
        public void OnActionExecutingShouldCreateAnErrorResponseIfArguementsContainsNullValue()
        {
            // Arrange
            var actionContext = MakeActionContext();
            actionContext.ActionArguments.Add("key", null);
            var sut = MakeSut();

            // Act
            sut.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            Assert.AreEqual(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
        }

        [TestMethod]
        public void OnActionExecutingShouldCreateAnErrorResponseIfModelStateIsInvalid()
        {
            // Arrange
            var actionContext = MakeActionContext();
            actionContext.ModelState.AddModelError("key", "errorMessage");
            var sut = MakeSut();

            // Act
            sut.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            Assert.AreEqual(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
        }

        private static HttpActionContext MakeActionContext()
        {
            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage()
            };
            var actionContext = new HttpActionContext(controllerContext, new ReflectedHttpActionDescriptor());

            return actionContext;
        }

        private static ModelValidationAttribute MakeSut()
        {
            var sut = new ModelValidationAttribute();

            return sut;
        }
    }
}
