using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("rmi")]
[ExcludeFromCodeCoverage]
public record DockerRmiOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Images) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--no-prune")]
    public bool? NoPrune { get; set; }
}
