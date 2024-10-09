using Microsoft.OpenApi.Models;

namespace AStar.ASPNet.Extensions;

/// <summary>
/// The <see cref="ApiConfiguration"/> class which is used to load the Api Configuration.
/// </summary>
public class ApiConfiguration
{
    /// <summary>
    /// The static Configuration Section Name which controls where DI looks for the API Configuration.
    /// </summary>
    public static string ConfigurationSectionName = "ApiConfiguration";

    /// <summary>
    /// The <see cref="OpenApiInfo"/> used to configure Swagger.
    /// </summary>
    public OpenApiInfo OpenApiInfo { get; set; } = new();
}
