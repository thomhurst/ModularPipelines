using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret inspect")]
public record DockerSecretInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Secret) : DockerOptions
{

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}