using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "increase-replica-count")]
public record AwsElasticacheIncreaseReplicaCountOptions(
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId
) : AwsOptions
{
    [CommandSwitch("--new-replica-count")]
    public int? NewReplicaCount { get; set; }

    [CommandSwitch("--replica-configuration")]
    public string[]? ReplicaConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}