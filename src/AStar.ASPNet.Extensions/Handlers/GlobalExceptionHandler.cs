using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AStar.ASPNet.Extensions.Handlers;

/// <summary>
/// The <see ref="GlobalExceptionMiddleware"></see> class contains the code to process any unhandled exceptions in a consistent, cross-solution, approach.
/// </summary>
/// <param name="logger">
/// An instance of <see href="ILogger"></see> used to log the error.
/// </param>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// </summary>
    /// <param name="httpContext">
    /// </param>
    /// <param name="exception">
    /// </param>
    /// <param name="cancellationToken">
    /// </param>
    /// <returns>
    /// </returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("An error occurred while processing your request: {Message}", exception.Message);

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Type = exception.GetType().Name,
            Title = "An unexpected error occurred",
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        }, CancellationToken.None);

        return true;
    }
}
