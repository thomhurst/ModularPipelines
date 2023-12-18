using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume inspect")]
[ExcludeFromCodeCoverage]
public record DockerVolumeInspectOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Volumes) : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}