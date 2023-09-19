using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin upgrade")]
[ExcludeFromCodeCoverage]
public record DockerPluginUpgradeOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Plugin) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Remote { get; set; }
    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandSwitch("--grant-all-permissions")]
    public string? GrantAllPermissions { get; set; }

    [CommandSwitch("--skip-remote-check")]
    public string? SkipRemoteCheck { get; set; }
}
