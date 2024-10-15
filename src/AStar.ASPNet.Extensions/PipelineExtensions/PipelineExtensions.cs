namespace AStar.ASPNet.Extensions.PipelineExtensions;

/// <summary>
/// The <see cref="ServiceCollectionExtensions" /> class contains the method(s) available to configure the pipeline in a consistent manner.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    /// The <see cref="ConfigurePipeline" /> will configure the pipeline to include Swagger, Authentication, Authorisation and basic live/ready health check endpoints
    /// </summary>
    /// <param name="webApplication">
    /// The instance of the <see cref="WebApplication" /> to configure.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WebApplication" /> to facilitate chaining.
    /// </returns>
    public static WebApplication ConfigurePipeline(this WebApplication webApplication)
    {
        _ = webApplication.UseExceptionHandler();
        _ = webApplication.UseSwagger();
        _ = webApplication.UseSwaggerUI(
           options =>
           {
               var descriptions = webApplication.DescribeApiVersions();

               // build a swagger endpoint for each discovered API version
               foreach(var description in descriptions)
               {
                   var url = $"/swagger/{description.GroupName}/swagger.json";
                   var name = description.GroupName.ToUpperInvariant();
                   options.SwaggerEndpoint(url, name);
               }
           });

        _ = webApplication.UseAuthentication()
                          .UseAuthorization();

        _ = webApplication.MapHealthChecks("/health");
        _ = webApplication.MapControllers();

        return webApplication;
    }
}
