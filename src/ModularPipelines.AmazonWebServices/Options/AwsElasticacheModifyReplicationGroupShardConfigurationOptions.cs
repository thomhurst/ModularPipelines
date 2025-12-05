using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-replication-group-shard-configuration")]
public record AwsElasticacheModifyReplicationGroupShardConfigurationOptions(
[property: CliOption("--replication-group-id")] string ReplicationGroupId,
[property: CliOption("--node-group-count")] int NodeGroupCount
) : AwsOptions
{
    [CliOption("--resharding-configuration")]
    public string[]? ReshardingConfiguration { get; set; }

    [CliOption("--node-groups-to-remove")]
    public string[]? NodeGroupsToRemove { get; set; }

    [CliOption("--node-groups-to-retain")]
    public string[]? NodeGroupsToRetain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}