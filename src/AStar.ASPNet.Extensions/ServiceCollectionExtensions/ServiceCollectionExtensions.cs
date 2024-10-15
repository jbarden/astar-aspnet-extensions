using Asp.Versioning;
using AStar.ASPNet.Extensions.Handlers;
using AStar.ASPNet.Extensions.Swagger;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AStar.ASPNet.Extensions.ServiceCollectionExtensions;

/// <summary>
/// The <see cref="ServiceCollectionExtensions" /> class contains the method(s) available to configure the pipeline in a consistent manner.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// The <see cref="ConfigureUi" /> will do exactly what it says on the tin...this time around, this is for the UI.
    /// </summary>
    /// <param name="services">
    /// An instance of the <see cref="IServiceCollection" /> interface that will be configured with the the Global Exception Handler, and the controllers (a UI isn't much use without them...).
    /// </param>
    /// <returns>
    /// The original <see cref="IServiceCollection" /> to facilitate method chaining.
    /// </returns>
    /// <seealso href="ConfigureApi">
    /// </seealso>
    public static IServiceCollection ConfigureUi(this IServiceCollection services)
    {
        _ = services
                .AddCommon()
                .AddControllers();

        return services;
    }

    /// <summary>
    /// The <see cref="ConfigureApi(IServiceCollection, OpenApiInfo, string)" /> will do exactly what it says on the tin... just, this time, it is API-specific.
    /// </summary>
    /// <param name="services">
    /// An instance of the <see cref="IServiceCollection" /> interface that will be configured with the current methods.
    /// </param>
    /// <param name="openApiInfo">
    /// The configured instance of <see cref="OpenApiInfo"/> to complete the Swagger configuration.
    /// </param>
    /// <param name="applicationName">
    /// The name of the application - this is used to configure Swagger.
    /// </param>
    /// <returns>
    /// The original <see cref="IServiceCollection" /> to facilitate method chaining.
    /// </returns>
    /// <seealso href="ConfigureUi">
    /// </seealso>
    /// <seealso href="ConfigureApi(IServiceCollection, OpenApiInfo)">
    /// </seealso>
    public static IServiceCollection ConfigureApi(this IServiceCollection services, OpenApiInfo openApiInfo, string applicationName)
    {
        _ = services.AddCommon()
                    .AddControllers(options => options.ReturnHttpNotAcceptable = true);

        _ = services.AddProblemDetails();
        _ = services.AddApiVersioning(options => options.ReportApiVersions = true)
                    .AddMvc()
                    .AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

        _ = services.AddTransient(_ => openApiInfo);
        _ = services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        _ = services.AddSwaggerGen(
            options =>
            {
                var fileName = $"{applicationName}.xml";
                var filePath = Path.Combine( AppContext.BaseDirectory, fileName );

                options.IncludeXmlComments(filePath);
            });

        services.TryAddEnumerable(ServiceDescriptor.Singleton<IProblemDetailsWriter, ErrorObjectWriter>());
        _ = services.AddHealthChecks();

        return services;
    }

    private static IServiceCollection AddCommon(this IServiceCollection services)
    {
        _ = services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
