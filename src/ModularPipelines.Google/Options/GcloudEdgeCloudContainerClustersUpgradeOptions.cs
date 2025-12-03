using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "upgrade")]
public record GcloudEdgeCloudContainerClustersUpgradeOptions : GcloudOptions
{
    public GcloudEdgeCloudContainerClustersUpgradeOptions(
        string cluster,
        string location,
        string schedule,
        string version
    )
    {
        Cluster = cluster;
        Location = location;
        Schedule = schedule;
        Version = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Cluster { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--schedule")]
    public string Schedule { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }
}
