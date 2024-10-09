using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AStar.ASPNet.Extensions.Swagger;

/// <summary>
/// Configures the Swagger generation options.
/// </summary>
/// <remarks>This allows API versioning to define a Swagger document per API version after the
/// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
/// </remarks>
/// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
/// <param name="openApiInfo">
/// The configured instance of <see cref="OpenApiInfo"/> to complete the Swagger configuration.
/// </param>
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, OpenApiInfo openApiInfo) : IConfigureOptions<SwaggerGenOptions>
{
    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        foreach(var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, openApiInfo));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, OpenApiInfo openApiInfo)
    {
        var text = new StringBuilder(openApiInfo.Description?? "Please update this default description: An example application with OpenAPI, Swashbuckle, and API versioning." );
        var info = new OpenApiInfo()
        {
            Title = openApiInfo.Title,
            Version = description.ApiVersion.ToString(),
            Contact = openApiInfo.Contact,
            License = openApiInfo.License    ,
            TermsOfService = openApiInfo.TermsOfService     ,
            Extensions = openApiInfo?.Extensions != null ? new Dictionary<string, IOpenApiExtension>(openApiInfo.Extensions) : null
        };

        if(description.IsDeprecated)
        {
            _ = text.Append(" This API version has been deprecated.");
        }

        if(description.SunsetPolicy is { } policy)
        {
            if(policy.Date is { } when)
            {
                _ = text.Append(" The API will be sunset on ")
                    .Append(when.Date.ToShortDateString())
                    .Append('.');
            }

            if(policy.HasLinks)
            {
                _ = text.AppendLine();

                var rendered = false;

                for(var i = 0; i < policy.Links.Count; i++)
                {
                    var link = policy.Links[i];

                    if(link.Type == "text/html")
                    {
                        if(!rendered)
                        {
                            _ = text.Append("<h4>Links</h4><ul>");
                            rendered = true;
                        }

                        _ = text.Append("<li><a href=\"");
                        _ = text.Append(link.LinkTarget.OriginalString);
                        _ = text.Append("\">");
                        _ = text.Append(
                            StringSegment.IsNullOrEmpty(link.Title)
                            ? link.LinkTarget.OriginalString
                            : link.Title.ToString());
                        _ = text.Append("</a></li>");
                    }
                }

                if(rendered)
                {
                    _ = text.Append("</ul>");
                }
            }
        }

        _ = text.Append("<h4>Additional Information</h4>");
        info.Description = text.ToString();

        return info;
    }
}
