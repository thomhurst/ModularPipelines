using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public bool? Build { get; set; }

    [CommandSwitch("--cap-add")]
    public string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public string? CapDrop { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-TTY")]
    public bool? NoTty { get; set; }

    [BooleanCommandSwitch("--no-deps")]
    public bool? NoDeps { get; set; }

    [CommandSwitch("--publish")]
    public string? Publish { get; set; }

    [BooleanCommandSwitch("--quiet-pull")]
    public bool? QuietPull { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public bool? RemoveOrphans { get; set; }

    [BooleanCommandSwitch("--rm")]
    public bool? Rm { get; set; }

    [CommandSwitch("--service-ports")]
    public string? ServicePorts { get; set; }

    [BooleanCommandSwitch("--tty")]
    public bool? Tty { get; set; }

    [CommandSwitch("--use-aliases")]
    public string? UseAliases { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--volume")]
    public string? Volume { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }
}
