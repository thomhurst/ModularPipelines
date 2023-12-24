using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents-data", "batch-reset-alarm")]
public record AwsIoteventsDataBatchResetAlarmOptions(
[property: CommandSwitch("--reset-action-requests")] string[] ResetActionRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}