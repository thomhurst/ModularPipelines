using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "start-inference-scheduler")]
public record AwsLookoutequipmentStartInferenceSchedulerOptions(
[property: CliOption("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}