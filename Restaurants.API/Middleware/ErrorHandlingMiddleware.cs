using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middleware
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException exception)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(exception.Message);
                logger.LogError(exception, exception.Message);

            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }

        }
    }
}
