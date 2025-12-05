using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-snapshot")]
public record AwsElasticacheCreateSnapshotOptions(
[property: CliOption("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CliOption("--replication-group-id")]
    public string? ReplicationGroupId { get; set; }

    [CliOption("--cache-cluster-id")]
    public string? CacheClusterId { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}