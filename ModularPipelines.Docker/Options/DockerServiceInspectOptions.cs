using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service inspect")]
public record DockerServiceInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Service) : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }
}