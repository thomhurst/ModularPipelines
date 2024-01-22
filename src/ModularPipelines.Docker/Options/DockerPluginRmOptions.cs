using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerPluginRmOptions : DockerOptions
{
    public DockerPluginRmOptions(
        IEnumerable<string> plugin
    )
    {
        CommandParts = ["plugin", "rm"];

        Plugin = plugin;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Plugin { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
