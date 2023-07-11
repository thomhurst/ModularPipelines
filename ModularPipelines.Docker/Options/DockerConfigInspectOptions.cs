using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config inspect")]
public record DockerConfigInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> ConfigNames) : DockerOptions
{
    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

}