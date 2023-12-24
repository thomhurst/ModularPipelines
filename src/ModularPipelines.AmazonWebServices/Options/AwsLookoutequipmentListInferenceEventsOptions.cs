using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "list-inference-events")]
public record AwsLookoutequipmentListInferenceEventsOptions(
[property: CommandSwitch("--inference-scheduler-name")] string InferenceSchedulerName,
[property: CommandSwitch("--interval-start-time")] long IntervalStartTime,
[property: CommandSwitch("--interval-end-time")] long IntervalEndTime
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}