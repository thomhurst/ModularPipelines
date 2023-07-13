using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image inspect")]
public record DockerImageInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Image) : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}