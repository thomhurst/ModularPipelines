using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "describe-document-classifier")]
public record AwsComprehendDescribeDocumentClassifierOptions(
[property: CliOption("--document-classifier-arn")] string DocumentClassifierArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}