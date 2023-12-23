using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "purchase-reserved-cache-nodes-offering")]
public record AwsElasticachePurchaseReservedCacheNodesOfferingOptions(
[property: CommandSwitch("--reserved-cache-nodes-offering-id")] string ReservedCacheNodesOfferingId
) : AwsOptions
{
    [CommandSwitch("--reserved-cache-node-id")]
    public string? ReservedCacheNodeId { get; set; }

    [CommandSwitch("--cache-node-count")]
    public int? CacheNodeCount { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}