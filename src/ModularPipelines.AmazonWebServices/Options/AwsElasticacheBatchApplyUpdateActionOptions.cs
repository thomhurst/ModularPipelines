using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "batch-apply-update-action")]
public record AwsElasticacheBatchApplyUpdateActionOptions(
[property: CommandSwitch("--service-update-name")] string ServiceUpdateName
) : AwsOptions
{
    [CommandSwitch("--replication-group-ids")]
    public string[]? ReplicationGroupIds { get; set; }

    [CommandSwitch("--cache-cluster-ids")]
    public string[]? CacheClusterIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}