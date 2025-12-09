using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "increase-replica-count")]
public record AwsElasticacheIncreaseReplicaCountOptions(
[property: CliOption("--replication-group-id")] string ReplicationGroupId
) : AwsOptions
{
    [CliOption("--new-replica-count")]
    public int? NewReplicaCount { get; set; }

    [CliOption("--replica-configuration")]
    public string[]? ReplicaConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}