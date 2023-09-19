using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context rm")]
[ExcludeFromCodeCoverage]
public record DockerContextRmOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Contexts) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
