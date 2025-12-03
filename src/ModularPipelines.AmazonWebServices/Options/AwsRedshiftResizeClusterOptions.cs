using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "resize-cluster")]
public record AwsRedshiftResizeClusterOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--cluster-type")]
    public string? ClusterType { get; set; }

    [CliOption("--node-type")]
    public string? NodeType { get; set; }

    [CliOption("--number-of-nodes")]
    public int? NumberOfNodes { get; set; }

    [CliOption("--reserved-node-id")]
    public string? ReservedNodeId { get; set; }

    [CliOption("--target-reserved-node-offering-id")]
    public string? TargetReservedNodeOfferingId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}