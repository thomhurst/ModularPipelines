using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents-data", "batch-snooze-alarm")]
public record AwsIoteventsDataBatchSnoozeAlarmOptions(
[property: CliOption("--snooze-action-requests")] string[] SnoozeActionRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}