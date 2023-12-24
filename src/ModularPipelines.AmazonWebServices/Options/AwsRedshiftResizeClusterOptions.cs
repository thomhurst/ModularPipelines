using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "resize-cluster")]
public record AwsRedshiftResizeClusterOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--cluster-type")]
    public string? ClusterType { get; set; }

    [CommandSwitch("--node-type")]
    public string? NodeType { get; set; }

    [CommandSwitch("--number-of-nodes")]
    public int? NumberOfNodes { get; set; }

    [CommandSwitch("--reserved-node-id")]
    public string? ReservedNodeId { get; set; }

    [CommandSwitch("--target-reserved-node-offering-id")]
    public string? TargetReservedNodeOfferingId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}