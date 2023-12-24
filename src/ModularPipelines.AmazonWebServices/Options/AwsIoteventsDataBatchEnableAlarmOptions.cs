using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents-data", "batch-enable-alarm")]
public record AwsIoteventsDataBatchEnableAlarmOptions(
[property: CommandSwitch("--enable-action-requests")] string[] EnableActionRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}