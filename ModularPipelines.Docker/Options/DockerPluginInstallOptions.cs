using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin install")]
public record DockerPluginInstallOptions : DockerOptions
{
    [CommandLongSwitch("alias")]
    public string? Alias { get; set; }

    [CommandLongSwitch("disable")]
    public string? Disable { get; set; }

    [BooleanCommandSwitch("disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandLongSwitch("grant-all-permissions")]
    public string? GrantAllPermissions { get; set; }

}