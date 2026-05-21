using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middleware;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.API.Tests.MIddlewares
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var nextDelegatesMock = new Mock<RequestDelegate>();
            //RequestDelegate next = (HttpContext ctx) => throw new Exception("Test exception");
            // Act
            await middleware.InvokeAsync(context, nextDelegatesMock.Object);
            // Assert
            nextDelegatesMock.Verify(next => next.Invoke(context), Times.Once);
        }
        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ReturnsSetStatusCode404()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var notFoundException = new NotFoundException(nameof(Restaurant), "1");
            //var nextDelegatesMock = new Mock<RequestDelegate>();

            //RequestDelegate next = (HttpContext ctx) => throw new Exception("Test exception");
            // Act
            await middleware.InvokeAsync(context, _ => throw notFoundException);
            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
        [Fact()]
        public async Task InvokeAsync_WhenForbidExceptionThrown_ReturnsSetStatusCode403()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var forbidxception = new ForbidException();
            //var nextDelegatesMock = new Mock<RequestDelegate>();

            //RequestDelegate next = (HttpContext ctx) => throw new Exception("Test exception");
            // Act
            await middleware.InvokeAsync(context, _ => throw forbidxception);
            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
        [Fact()]
        public async Task InvokeAsync_WhenGenericExceptionThrown_ReturnsSetStatusCode500()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var exception = new  Exception();
            //var nextDelegatesMock = new Mock<RequestDelegate>();

            //RequestDelegate next = (HttpContext ctx) => throw new Exception("Test exception");
            // Act
            await middleware.InvokeAsync(context, _ => throw exception);
            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
