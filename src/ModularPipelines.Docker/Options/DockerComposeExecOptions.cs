using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerComposeExecOptions : DockerOptions
{
    public DockerComposeExecOptions(
        string service,
        string command
    )
    {
        CommandParts = ["compose", "exec"];

        Service = service;

        Command = command;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Service { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Command { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Args { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [CommandSwitch("--index")]
    public virtual string? Index { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-TTY")]
    public virtual bool? NoTty { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public virtual bool? Privileged { get; set; }

    [BooleanCommandSwitch("--tty")]
    public virtual bool? Tty { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--workdir")]
    public virtual string? Workdir { get; set; }
}
