using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "node-pools", "list")]
public record GcloudEdgeCloudContainerClustersNodePoolsListOptions : GcloudOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}