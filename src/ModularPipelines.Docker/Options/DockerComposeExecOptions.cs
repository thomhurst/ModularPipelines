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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Service { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Command { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Args { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliOption("--index")]
    public virtual string? Index { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-TTY")]
    public virtual bool? NoTty { get; set; }

    [CliFlag("--privileged")]
    public virtual bool? Privileged { get; set; }

    [CliFlag("--tty")]
    public virtual bool? Tty { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--workdir")]
    public virtual string? Workdir { get; set; }
}
