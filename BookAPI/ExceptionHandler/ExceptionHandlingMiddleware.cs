using Newtonsoft.Json;
using System.Net;

namespace BookAPI.ExceptionHandler
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                error = "An unexpected error occurred.",
                details = exception.Message
            };

            var json = JsonConvert.SerializeObject(errorResponse);

            await context.Response.WriteAsync(json);
        }
    }
}
