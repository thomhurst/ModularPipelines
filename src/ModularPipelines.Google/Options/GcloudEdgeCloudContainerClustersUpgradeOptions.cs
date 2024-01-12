using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "clusters", "upgrade")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Cluster { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--schedule")]
    public string Schedule { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }
}
