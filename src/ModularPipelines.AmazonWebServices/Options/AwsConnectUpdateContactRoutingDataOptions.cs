using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-contact-routing-data")]
public record AwsConnectUpdateContactRoutingDataOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--contact-id")] string ContactId
) : AwsOptions
{
    [CommandSwitch("--queue-time-adjustment-seconds")]
    public int? QueueTimeAdjustmentSeconds { get; set; }

    [CommandSwitch("--queue-priority")]
    public long? QueuePriority { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}