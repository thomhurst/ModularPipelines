using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin upgrade")]
public record DockerPluginUpgradeOptions : DockerOptions
{
    [BooleanCommandSwitch("disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandLongSwitch("grant-all-permissions")]
    public string? GrantAllPermissions { get; set; }

    [CommandLongSwitch("skip-remote-check")]
    public string? SkipRemoteCheck { get; set; }

}