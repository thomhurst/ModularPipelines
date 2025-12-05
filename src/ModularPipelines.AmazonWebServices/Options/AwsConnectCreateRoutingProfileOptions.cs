using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-routing-profile")]
public record AwsConnectCreateRoutingProfileOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--description")] string Description,
[property: CliOption("--default-outbound-queue-id")] string DefaultOutboundQueueId,
[property: CliOption("--media-concurrencies")] string[] MediaConcurrencies
) : AwsOptions
{
    [CliOption("--queue-configs")]
    public string[]? QueueConfigs { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--agent-availability-timer")]
    public string? AgentAvailabilityTimer { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}