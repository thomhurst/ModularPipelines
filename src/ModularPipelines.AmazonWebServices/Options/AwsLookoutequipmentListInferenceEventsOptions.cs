using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "list-inference-events")]
public record AwsLookoutequipmentListInferenceEventsOptions(
[property: CliOption("--inference-scheduler-name")] string InferenceSchedulerName,
[property: CliOption("--interval-start-time")] long IntervalStartTime,
[property: CliOption("--interval-end-time")] long IntervalEndTime
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}