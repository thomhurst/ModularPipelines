using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "clusters", "upgrade")]
public record GcloudContainerVmwareClustersUpgradeOptions : GcloudOptions
{
    public GcloudContainerVmwareClustersUpgradeOptions(
        string cluster,
        string location,
        string version
    )
    {
        Cluster = cluster;
        Location = location;
        Version = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Cluster { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }
}
