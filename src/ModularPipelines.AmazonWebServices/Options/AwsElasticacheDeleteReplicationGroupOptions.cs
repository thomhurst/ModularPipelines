using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "delete-replication-group")]
public record AwsElasticacheDeleteReplicationGroupOptions(
[property: CliOption("--replication-group-id")] string ReplicationGroupId
) : AwsOptions
{
    [CliOption("--final-snapshot-identifier")]
    public string? FinalSnapshotIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}