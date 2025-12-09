using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "batch-apply-update-action")]
public record AwsElasticacheBatchApplyUpdateActionOptions(
[property: CliOption("--service-update-name")] string ServiceUpdateName
) : AwsOptions
{
    [CliOption("--replication-group-ids")]
    public string[]? ReplicationGroupIds { get; set; }

    [CliOption("--cache-cluster-ids")]
    public string[]? CacheClusterIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}