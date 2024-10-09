﻿using System.Diagnostics;
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
    /// The TryHandleAsync as defined by the <see cref="IExceptionHandler"/> interface.
    /// </summary>
    /// <param name="httpContext">
    /// The <see cref="HttpContext"/> for the current request.
    /// </param>
    /// <param name="exception">
    /// The unhandled exception.
    /// </param>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> that the framework will pass to the method.
    /// </param>
    /// <returns>
    /// An instance of <see cref="ValueTask"/> of type <see cref="bool"/> as defined by the <see cref="IExceptionHandler"/> interface.
    /// </returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogError(exception, "An error occurred while processing a request. The message is: {Message}", exception.Message);
            var detailMessage = "We are sorry, but our server encountered an error. This has been logged and our support team will resolve as quickly as possible. If you wish to contact the support team, please quote the traceId listed below. Thank you for your patience. AStar Development.";

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Internal Server Error",
                Title = "An unexpected error occurred",
                Detail = detailMessage,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Extensions = {
            { "traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier }
        }
            }, CancellationToken.None);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
