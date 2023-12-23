using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-replication-group-shard-configuration")]
public record AwsElasticacheModifyReplicationGroupShardConfigurationOptions(
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId,
[property: CommandSwitch("--node-group-count")] int NodeGroupCount
) : AwsOptions
{
    [CommandSwitch("--resharding-configuration")]
    public string[]? ReshardingConfiguration { get; set; }

    [CommandSwitch("--node-groups-to-remove")]
    public string[]? NodeGroupsToRemove { get; set; }

    [CommandSwitch("--node-groups-to-retain")]
    public string[]? NodeGroupsToRetain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}