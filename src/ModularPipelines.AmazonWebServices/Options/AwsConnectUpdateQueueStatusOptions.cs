using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-queue-status")]
public record AwsConnectUpdateQueueStatusOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--queue-id")] string QueueId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}