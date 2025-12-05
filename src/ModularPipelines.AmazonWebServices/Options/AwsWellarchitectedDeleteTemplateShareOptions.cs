using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "delete-template-share")]
public record AwsWellarchitectedDeleteTemplateShareOptions(
[property: CliOption("--share-id")] string ShareId,
[property: CliOption("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}