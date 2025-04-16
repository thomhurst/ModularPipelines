using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginInstallOptions : DockerOptions
{
    public DockerPluginInstallOptions(
        string plugin
    )
    {
        CommandParts = ["plugin", "install"];

        Plugin = plugin;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Plugin { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<KeyValue>? KeyValue { get; set; }

    [CommandSwitch("--alias")]
    public virtual string? Alias { get; set; }

    [CommandSwitch("--disable")]
    public virtual string? Disable { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CommandSwitch("--grant-all-permissions")]
    public virtual string? GrantAllPermissions { get; set; }
}
