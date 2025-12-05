using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-push-template")]
public record AwsPinpointGetPushTemplateOptions(
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--template-version")]
    public string? TemplateVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}