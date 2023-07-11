using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin upgrade")]
public record DockerPluginUpgradeOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Plugin) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Remote { get; set; }
    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandSwitch("--grant-all-permissions")]
    public string? GrantAllPermissions { get; set; }

    [CommandSwitch("--skip-remote-check")]
    public string? SkipRemoteCheck { get; set; }

}