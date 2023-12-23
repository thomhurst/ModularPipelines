using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents-data", "batch-acknowledge-alarm")]
public record AwsIoteventsDataBatchAcknowledgeAlarmOptions(
[property: CommandSwitch("--acknowledge-action-requests")] string[] AcknowledgeActionRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}