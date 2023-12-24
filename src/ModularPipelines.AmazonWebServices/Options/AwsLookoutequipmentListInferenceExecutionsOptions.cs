using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "list-inference-executions")]
public record AwsLookoutequipmentListInferenceExecutionsOptions(
[property: CommandSwitch("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--data-start-time-after")]
    public long? DataStartTimeAfter { get; set; }

    [CommandSwitch("--data-end-time-before")]
    public long? DataEndTimeBefore { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}