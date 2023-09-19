using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container inspect")]
[ExcludeFromCodeCoverage]
public record DockerContainerInspectOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Containers) : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}
