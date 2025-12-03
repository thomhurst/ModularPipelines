using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "test-render-template")]
public record AwsSesTestRenderTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-data")] string TemplateData
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}