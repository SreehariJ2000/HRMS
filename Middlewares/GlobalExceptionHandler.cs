using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Log the unhandled exception
            _logger.LogError(exception, "A global unhandled exception occurred! Path: {Path}, Method: {Method}",
                httpContext.Request.Path, httpContext.Request.Method);

            // 2. Identify if the request was an AJAX/API call
            var isAjax = httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while processing your request. Please try again later.",
                    Instance = httpContext.Request.Path
                };

                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            }
            else
            {
                // 3. For traditional UI requests, redirect to the Error View
                httpContext.Response.Redirect("/Home/Error");
            }

            // Return true to indicate the exception has been successfully handled
            // and the pipeline should short-circuit here.
            return true;
        }
    }
}
