using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin install")]
[ExcludeFromCodeCoverage]
public record DockerPluginInstallOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Plugin) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string>? KeyValues { get; set; }

    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--disable")]
    public string? Disable { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandSwitch("--grant-all-permissions")]
    public string? GrantAllPermissions { get; set; }
}
