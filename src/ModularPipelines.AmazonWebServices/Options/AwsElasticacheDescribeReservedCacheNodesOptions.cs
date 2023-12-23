using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "describe-reserved-cache-nodes")]
public record AwsElasticacheDescribeReservedCacheNodesOptions : AwsOptions
{
    [CommandSwitch("--reserved-cache-node-id")]
    public string? ReservedCacheNodeId { get; set; }

    [CommandSwitch("--reserved-cache-nodes-offering-id")]
    public string? ReservedCacheNodesOfferingId { get; set; }

    [CommandSwitch("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--product-description")]
    public string? ProductDescription { get; set; }

    [CommandSwitch("--offering-type")]
    public string? OfferingType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}