using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "list-allowed-node-type-modifications")]
public record AwsElasticacheListAllowedNodeTypeModificationsOptions : AwsOptions
{
    [CommandSwitch("--cache-cluster-id")]
    public string? CacheClusterId { get; set; }

    [CommandSwitch("--replication-group-id")]
    public string? ReplicationGroupId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}