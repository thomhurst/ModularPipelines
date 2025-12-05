using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-template-active-version")]
public record AwsPinpointUpdateTemplateActiveVersionOptions(
[property: CliOption("--template-active-version-request")] string TemplateActiveVersionRequest,
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-type")] string TemplateType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}