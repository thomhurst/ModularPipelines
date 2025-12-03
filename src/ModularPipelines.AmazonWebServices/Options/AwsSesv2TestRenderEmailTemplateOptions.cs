using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "test-render-email-template")]
public record AwsSesv2TestRenderEmailTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-data")] string TemplateData
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}