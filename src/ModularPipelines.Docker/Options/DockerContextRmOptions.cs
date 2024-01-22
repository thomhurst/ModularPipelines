using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context", "rm")]
[ExcludeFromCodeCoverage]
public record DockerContextRmOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? RmContext { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
