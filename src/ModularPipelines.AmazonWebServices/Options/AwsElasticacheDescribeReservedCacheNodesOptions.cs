using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "describe-reserved-cache-nodes")]
public record AwsElasticacheDescribeReservedCacheNodesOptions : AwsOptions
{
    [CliOption("--reserved-cache-node-id")]
    public string? ReservedCacheNodeId { get; set; }

    [CliOption("--reserved-cache-nodes-offering-id")]
    public string? ReservedCacheNodesOfferingId { get; set; }

    [CliOption("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--product-description")]
    public string? ProductDescription { get; set; }

    [CliOption("--offering-type")]
    public string? OfferingType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}