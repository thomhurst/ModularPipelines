using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container inspect")]
public record DockerContainerInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}