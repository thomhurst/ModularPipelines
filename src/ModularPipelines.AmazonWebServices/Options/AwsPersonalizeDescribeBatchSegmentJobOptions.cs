using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-batch-segment-job")]
public record AwsPersonalizeDescribeBatchSegmentJobOptions(
[property: CliOption("--batch-segment-job-arn")] string BatchSegmentJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}