using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-routing-profile")]
public record AwsConnectCreateRoutingProfileOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--default-outbound-queue-id")] string DefaultOutboundQueueId,
[property: CommandSwitch("--media-concurrencies")] string[] MediaConcurrencies
) : AwsOptions
{
    [CommandSwitch("--queue-configs")]
    public string[]? QueueConfigs { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--agent-availability-timer")]
    public string? AgentAvailabilityTimer { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}