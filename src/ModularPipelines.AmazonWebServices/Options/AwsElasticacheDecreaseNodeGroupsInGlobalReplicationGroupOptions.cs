using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "decrease-node-groups-in-global-replication-group")]
public record AwsElasticacheDecreaseNodeGroupsInGlobalReplicationGroupOptions(
[property: CommandSwitch("--global-replication-group-id")] string GlobalReplicationGroupId,
[property: CommandSwitch("--node-group-count")] int NodeGroupCount
) : AwsOptions
{
    [CommandSwitch("--global-node-groups-to-remove")]
    public string[]? GlobalNodeGroupsToRemove { get; set; }

    [CommandSwitch("--global-node-groups-to-retain")]
    public string[]? GlobalNodeGroupsToRetain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}