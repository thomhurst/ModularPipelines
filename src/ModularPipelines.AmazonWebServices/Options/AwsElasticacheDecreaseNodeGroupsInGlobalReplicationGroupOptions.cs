using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "decrease-node-groups-in-global-replication-group")]
public record AwsElasticacheDecreaseNodeGroupsInGlobalReplicationGroupOptions(
[property: CliOption("--global-replication-group-id")] string GlobalReplicationGroupId,
[property: CliOption("--node-group-count")] int NodeGroupCount
) : AwsOptions
{
    [CliOption("--global-node-groups-to-remove")]
    public string[]? GlobalNodeGroupsToRemove { get; set; }

    [CliOption("--global-node-groups-to-retain")]
    public string[]? GlobalNodeGroupsToRetain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}