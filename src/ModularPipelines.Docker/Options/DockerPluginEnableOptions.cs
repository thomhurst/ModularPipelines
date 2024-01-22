using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginEnableOptions : DockerOptions
{
    public DockerPluginEnableOptions(
        string plugin
    )
    {
        CommandParts = ["plugin", "enable"];

        Plugin = plugin;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Plugin { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }
}
