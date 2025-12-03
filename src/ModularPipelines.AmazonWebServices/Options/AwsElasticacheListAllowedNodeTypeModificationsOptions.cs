using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "list-allowed-node-type-modifications")]
public record AwsElasticacheListAllowedNodeTypeModificationsOptions : AwsOptions
{
    [CliOption("--cache-cluster-id")]
    public string? CacheClusterId { get; set; }

    [CliOption("--replication-group-id")]
    public string? ReplicationGroupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}