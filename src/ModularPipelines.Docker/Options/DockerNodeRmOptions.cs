using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node rm")]
[ExcludeFromCodeCoverage]
public record DockerNodeRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Node) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
