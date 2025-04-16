using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerExecOptions : DockerOptions
{
    public DockerContainerExecOptions(
        string container,
        string command
    )
    {
        CommandParts = ["container", "exec"];

        Container = container;

        Command = command;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Command { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Arg { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--detach-keys")]
    public virtual string? DetachKeys { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [CommandSwitch("--env-file")]
    public virtual string? EnvFile { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public virtual bool? Privileged { get; set; }

    [CommandSwitch("--tty")]
    public virtual string? Tty { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--workdir")]
    public virtual string? Workdir { get; set; }
}
