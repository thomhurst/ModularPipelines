using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume inspect")]
public record DockerVolumeInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Volume) : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}