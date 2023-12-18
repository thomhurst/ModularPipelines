using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image inspect")]
[ExcludeFromCodeCoverage]
public record DockerImageInspectOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Images) : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}