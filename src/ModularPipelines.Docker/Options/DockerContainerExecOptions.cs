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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Container { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Command { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Arg { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--detach-keys")]
    public virtual string? DetachKeys { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliOption("--env-file")]
    public virtual string? EnvFile { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--privileged")]
    public virtual bool? Privileged { get; set; }

    [CliOption("--tty")]
    public virtual string? Tty { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--workdir")]
    public virtual string? Workdir { get; set; }
}
