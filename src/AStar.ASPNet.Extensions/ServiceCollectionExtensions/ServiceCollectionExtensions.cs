using System.Text.Json.Serialization;
using AStar.ASPNet.Extensions.Handlers;
using AStar.Logging.Extensions;

namespace AStar.ASPNet.Extensions.ServiceCollectionExtensions;

/// <summary>
/// The <see cref="ServiceCollectionExtensions"/> class contains the method(s) available to configure the pipeline in a consistent manner. 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// The <see cref="ConfigurePipeline"/> will do exactly what it says on the tin... 
    /// </summary>
    /// <param name="services">An instance of the <see cref="IServiceCollection"/> interface that will be configured with the current methods.</param>
    /// <returns>The original <see cref="IServiceCollection"/> to facilitate method chaining.   </returns>
    public static IServiceCollection ConfigurePipeline(this IServiceCollection services)
    {
        _ = services.AddExceptionHandler<GlobalExceptionHandler>(); 
        _ = services.AddControllers()
                    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        _ = services.AddEndpointsApiExplorer();
        _ = services.AddSwaggerGen();
        _ = services.AddHealthChecks();

        return services;
    }

    /// <summary>
    /// The <see cref="AddLogging"/> will do exactly what it says on the tin... 
    /// </summary>
    /// <param name="builder">An instance of the <see cref="WebApplicationBuilder"/> class that will be configured with the current methods.</param>
    /// <returns>The original <see cref="WebApplicationBuilder"/> to facilitate method chaining.   </returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        _ = builder.UseSerilogLogging();

        return builder;
    }
}