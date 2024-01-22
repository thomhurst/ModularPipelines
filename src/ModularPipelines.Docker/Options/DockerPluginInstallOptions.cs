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
    public string? Alias { get; set; }

    [CommandSwitch("--disable")]
    public string? Disable { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandSwitch("--grant-all-permissions")]
    public string? GrantAllPermissions { get; set; }
}
