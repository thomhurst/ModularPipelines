using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginInspectOptions : DockerOptions
{
    public DockerPluginInspectOptions(
        IEnumerable<string> plugin
    )
    {
        CommandParts = ["plugin", "inspect"];

        Plugin = plugin;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Plugin { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }
}
