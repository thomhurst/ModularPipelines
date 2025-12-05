using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "increase-node-groups-in-global-replication-group")]
public record AwsElasticacheIncreaseNodeGroupsInGlobalReplicationGroupOptions(
[property: CliOption("--global-replication-group-id")] string GlobalReplicationGroupId,
[property: CliOption("--node-group-count")] int NodeGroupCount
) : AwsOptions
{
    [CliOption("--regional-configurations")]
    public string[]? RegionalConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}