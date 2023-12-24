using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "increase-node-groups-in-global-replication-group")]
public record AwsElasticacheIncreaseNodeGroupsInGlobalReplicationGroupOptions(
[property: CommandSwitch("--global-replication-group-id")] string GlobalReplicationGroupId,
[property: CommandSwitch("--node-group-count")] int NodeGroupCount
) : AwsOptions
{
    [CommandSwitch("--regional-configurations")]
    public string[]? RegionalConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}