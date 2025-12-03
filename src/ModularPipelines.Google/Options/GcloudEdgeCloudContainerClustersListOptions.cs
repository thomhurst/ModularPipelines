using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "list")]
public record GcloudEdgeCloudContainerClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}