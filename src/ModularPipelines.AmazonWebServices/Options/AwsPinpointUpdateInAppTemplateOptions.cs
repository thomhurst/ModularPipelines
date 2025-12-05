using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-in-app-template")]
public record AwsPinpointUpdateInAppTemplateOptions(
[property: CliOption("--in-app-template-request")] string InAppTemplateRequest,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--template-version")]
    public string? TemplateVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}