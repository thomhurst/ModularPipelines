using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-queue-outbound-caller-config")]
public record AwsConnectUpdateQueueOutboundCallerConfigOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--queue-id")] string QueueId,
[property: CommandSwitch("--outbound-caller-config")] string OutboundCallerConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}