using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents-data", "batch-snooze-alarm")]
public record AwsIoteventsDataBatchSnoozeAlarmOptions(
[property: CommandSwitch("--snooze-action-requests")] string[] SnoozeActionRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}