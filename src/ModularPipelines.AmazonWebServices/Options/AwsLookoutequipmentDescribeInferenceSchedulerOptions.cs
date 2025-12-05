using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "describe-inference-scheduler")]
public record AwsLookoutequipmentDescribeInferenceSchedulerOptions(
[property: CliOption("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}