using System.Text.Json.Serialization;
using AStar.ASPNet.Extensions.Handlers;
using AStar.Logging.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AStar.ASPNet.Extensions.ServiceCollectionExtensions;

/// <summary>
/// The <see cref="ServiceCollectionExtensions"/> class contains the method(s) available to configure the pipeline in a consistent manner.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// The <see cref="Configure"/> will do exactly what it says on the tin...
    /// </summary>
    /// <param name="services">An instance of the <see cref="IServiceCollection"/> interface that will be configured with the current methods.</param>
    /// <returns>The original <see cref="IServiceCollection"/> to facilitate method chaining.   </returns>
    public static IServiceCollection Configure(this IServiceCollection services)
    {
        _ = services.AddExceptionHandler<GlobalExceptionHandler>();
        _ = services.AddControllers(options =>
        {
            options.ReturnHttpNotAcceptable = true;
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
        })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        _ = services.AddEndpointsApiExplorer();
        _ = services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.EnableAnnotations();
        });
        _ = services.AddHealthChecks();

        return services;
    }

    /// <summary>
    /// The <see cref="AddLogging"/> will do exactly what it says on the tin...
    /// </summary>
    /// <param name="builder">An instance of the <see cref="WebApplicationBuilder"/> class that will be configured with the current methods.</param>
    /// <param name="externalSettingsFile">
    /// The name (including extension) of the file containing the Serilog Configuration settings.
    /// </param>
    /// <returns>The original <see cref="WebApplicationBuilder"/> to facilitate method chaining.</returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder, string externalSettingsFile = "")
    {
        _ = builder.UseSerilogLogging(externalSettingsFile);

        return builder;
    }
}
