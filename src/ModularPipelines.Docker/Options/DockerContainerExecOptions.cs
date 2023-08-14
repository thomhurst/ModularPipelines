using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container exec")]
public record DockerContainerExecOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container, [property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string>? DockerArgs { get; set; }
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--detach-keys")]
    public string? DetachKeys { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--env-file")]
    public string? EnvFile { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public bool? Privileged { get; set; }

    [CommandSwitch("--tty")]
    public string? Tty { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }
}