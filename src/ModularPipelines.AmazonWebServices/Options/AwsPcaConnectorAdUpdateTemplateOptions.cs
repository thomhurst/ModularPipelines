using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pca-connector-ad", "update-template")]
public record AwsPcaConnectorAdUpdateTemplateOptions(
[property: CliOption("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}