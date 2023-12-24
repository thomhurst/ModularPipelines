using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "delete-cache-cluster")]
public record AwsElasticacheDeleteCacheClusterOptions(
[property: CommandSwitch("--cache-cluster-id")] string CacheClusterId
) : AwsOptions
{
    [CommandSwitch("--final-snapshot-identifier")]
    public string? FinalSnapshotIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}