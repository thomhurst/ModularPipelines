using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerComposeRunOptions : DockerOptions
{
    public DockerComposeRunOptions(
        string service
    )
    {
        CommandParts = ["compose", "run"];

        Service = service;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Service { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Command { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Args { get; set; }

    [BooleanCommandSwitch("--build")]
    public virtual bool? Build { get; set; }

    [CommandSwitch("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--label")]
    public virtual string? Label { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [BooleanCommandSwitch("--no-TTY")]
    public virtual bool? NoTty { get; set; }

    [BooleanCommandSwitch("--no-deps")]
    public virtual bool? NoDeps { get; set; }

    [CommandSwitch("--publish")]
    public virtual string? Publish { get; set; }

    [BooleanCommandSwitch("--quiet-pull")]
    public virtual bool? QuietPull { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [BooleanCommandSwitch("--rm")]
    public virtual bool? Rm { get; set; }

    [CommandSwitch("--service-ports")]
    public virtual string? ServicePorts { get; set; }

    [BooleanCommandSwitch("--tty")]
    public virtual bool? Tty { get; set; }

    [CommandSwitch("--use-aliases")]
    public virtual string? UseAliases { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--volume")]
    public virtual string? Volume { get; set; }

    [CommandSwitch("--workdir")]
    public virtual string? Workdir { get; set; }
}
