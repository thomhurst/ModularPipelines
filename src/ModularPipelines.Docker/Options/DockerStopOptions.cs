using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stop")]
[ExcludeFromCodeCoverage]
public record DockerStopOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Containers) : DockerOptions
{
    [CommandSwitch("--signal")]
    public string? Signal { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }
}
