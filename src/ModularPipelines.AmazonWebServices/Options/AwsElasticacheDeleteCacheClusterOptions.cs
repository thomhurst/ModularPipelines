using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "delete-cache-cluster")]
public record AwsElasticacheDeleteCacheClusterOptions(
[property: CliOption("--cache-cluster-id")] string CacheClusterId
) : AwsOptions
{
    [CliOption("--final-snapshot-identifier")]
    public string? FinalSnapshotIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}