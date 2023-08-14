using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose exec")]
public record DockerComposeExecOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Service, [property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string>? Args { get; set; }
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [BooleanCommandSwitch("--no-TTY")]
    public bool? NoTTY { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public bool? Privileged { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}