using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "purchase-reserved-cache-nodes-offering")]
public record AwsElasticachePurchaseReservedCacheNodesOfferingOptions(
[property: CliOption("--reserved-cache-nodes-offering-id")] string ReservedCacheNodesOfferingId
) : AwsOptions
{
    [CliOption("--reserved-cache-node-id")]
    public string? ReservedCacheNodeId { get; set; }

    [CliOption("--cache-node-count")]
    public int? CacheNodeCount { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}