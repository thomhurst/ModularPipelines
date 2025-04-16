using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginUpgradeOptions : DockerOptions
{
    public DockerPluginUpgradeOptions(
        string plugin
    )
    {
        CommandParts = ["plugin", "upgrade"];

        Plugin = plugin;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Plugin { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Remote { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CommandSwitch("--grant-all-permissions")]
    public virtual string? GrantAllPermissions { get; set; }

    [CommandSwitch("--skip-remote-check")]
    public virtual string? SkipRemoteCheck { get; set; }
}
