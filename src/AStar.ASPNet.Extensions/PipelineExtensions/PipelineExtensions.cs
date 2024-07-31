namespace AStar.ASPNet.Extensions.PipelineExtensions;

/// <summary>
/// The <see cref="ServiceCollectionExtensions"/> class contains the method(s) available to configure the pipeline in a consistent manner.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    /// The <see cref="ConfigurePipeline"/> will configure the pipeline to include Swagger, Authentication, Authorisation and basic live/ready health check endpoints
    /// </summary>
    /// <param name="webApplication">The instance of the <see cref="WebApplication"/> to configure.</param>
    /// <param name="registerSwagger">The registerSwagger parameter will, as you can reasonably expect, add the relevant UseSwagger commands. The default is <c>true</c>.
    /// Set this parameter to false if you are using Fast Endpoints (or any other package that configures swagger separately).</param>
    /// <returns></returns>
    public static WebApplication ConfigurePipeline(this WebApplication webApplication, bool registerSwagger = true)
    {
        if(registerSwagger)
        {
            _ = webApplication.UseSwagger()
                            .UseSwaggerUI();    
        }
        
        _ = webApplication.UseAuthentication()
                          .UseAuthorization();
        //.UseResponseCaching();
        //.UseHttpCacheHeaders();

        _ = webApplication.MapHealthChecks("/health");
        _ = webApplication.UseExceptionHandler(opt => { });
        _ = webApplication.MapControllers();

        return webApplication;
    }
}
