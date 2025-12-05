using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "update-email-template")]
public record AwsSesv2UpdateEmailTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-content")] string TemplateContent
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}