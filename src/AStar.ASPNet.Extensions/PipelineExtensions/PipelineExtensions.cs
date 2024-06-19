using AStar.Api.HealthChecks;

namespace AStar.ASPNet.Extensions.PipelineExtensions;

/// <summary>
/// The <see cref="ServiceCollectionExtensions"/> class contains the method(s) available to configure the pipeline in a consistent manner.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    /// The <see cref="ConfigurePipeline"/> will configure the pipeline to include Swagger, Authentication, Authorisation and basic live/ready health check endpoints
    /// </summary>
    /// <param name="webApplication"></param>
    /// <returns></returns>
    public static WebApplication ConfigurePipeline(this WebApplication webApplication)
    {
        _ = webApplication.UseSwagger()
                          .UseSwaggerUI()
                          .UseAuthentication();
        //.UseAuthorization();
        //.UseResponseCaching();
        //.UseHttpCacheHeaders();

        _ = webApplication.ConfigureHealthCheckEndpoints();
        _ = webApplication.UseExceptionHandler(opt => { });
        _ = webApplication.MapControllers();

        return webApplication;
    }
}
