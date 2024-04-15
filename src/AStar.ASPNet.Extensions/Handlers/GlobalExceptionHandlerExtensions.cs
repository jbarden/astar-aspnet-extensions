namespace AStar.ASPNet.Extensions.Handlers;

/// <summary>
/// The <see href="GlobalExceptionHandlerExtensions"></see> class centralises the Global Exception handler consumption.
/// </summary>
public static class GlobalExceptionHandlerExtensions
{
    /// <summary>
    /// This method extends the <see href="IServiceCollection"></see> to allow for a fluent, simpler addition of the custom <see href="GlobalExceptionHandler"></see> middleware.
    /// </summary>
    /// <param name="services">
    /// </param>
    /// <returns>
    /// The original <see href="IApplicationBuilder"></see> instance for further method chaining.
    /// </returns>
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
                                            => services.AddExceptionHandler<GlobalExceptionHandler>();
}
