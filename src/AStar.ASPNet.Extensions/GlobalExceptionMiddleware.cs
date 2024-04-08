using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace AStar.ASPNet.Extensions;

/// <summary>
/// The <see ref="GlobalExceptionMiddleware"></see> class contains the code to process any unhandled exceptions in a consistent, cross-solution, approach.
/// </summary>
/// <param name="next">
/// An instance of <see ref="RequestDelegate"></see> that contains the next delegate in the ASP.Net pipeline.
/// </param>
public class GlobalExceptionMiddleware(RequestDelegate next)
{
    /// <summary>
    /// This method will handle any unhandled exception and return a standardised message.
    /// </summary>
    /// <param name="context">
    /// An instance of <see href="HttpContext"></see> containing the HTTP Context for the current request.
    /// </param>
    /// <returns>
    /// An awaitable task.
    /// </returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                ApplicationException e => (int)HttpStatusCode.BadRequest,// custom application error
                KeyNotFoundException e => (int)HttpStatusCode.NotFound,// not found error
                _ => 418,// unhandled error
            };
            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}
