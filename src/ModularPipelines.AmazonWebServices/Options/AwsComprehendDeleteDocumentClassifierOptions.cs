using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "delete-document-classifier")]
public record AwsComprehendDeleteDocumentClassifierOptions(
[property: CliOption("--document-classifier-arn")] string DocumentClassifierArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}