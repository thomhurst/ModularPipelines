using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "clusters", "upgrade")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Cluster { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }
}
