using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-batch-inference-job")]
public record AwsPersonalizeDescribeBatchInferenceJobOptions(
[property: CliOption("--batch-inference-job-arn")] string BatchInferenceJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}