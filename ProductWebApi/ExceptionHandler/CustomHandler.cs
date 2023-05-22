using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductWebApi.ExceptionHandler
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomHandler
    {
        private readonly RequestDelegate _next;

        public CustomHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Log.Fatal("sfdgdfgdf           dsfdsf");
                await HandleExceptionMessageAsync(httpContext, ex).ConfigureAwait(false);
            }
        }
        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {

            context.Response.ContentType = "application/json";
            int statusCode = 404;// (int)HttpStatusCode.NotFound;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message + " this is inner exception"
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomHandlerExtensions
    {
        public static IApplicationBuilder UseCustomHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomHandler>();
        }
    }
}
