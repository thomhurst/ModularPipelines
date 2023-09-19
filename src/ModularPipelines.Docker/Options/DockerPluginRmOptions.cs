using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin rm")]
[ExcludeFromCodeCoverage]
public record DockerPluginRmOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Plugins) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
