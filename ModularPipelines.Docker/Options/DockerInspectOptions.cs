using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("inspect")]
public record DockerInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Name) : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}