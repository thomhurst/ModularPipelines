using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "describe-document-classification-job")]
public record AwsComprehendDescribeDocumentClassificationJobOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}