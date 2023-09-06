using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose run")]
[ExcludeFromCodeCoverage]
public record DockerComposeRunOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Service) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Command { get; set; }

    [PositionalArgument(Position = Position.AfterArguments)]
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
    public bool? NoTTY { get; set; }

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

    [CommandSwitch("--use-aliases")]
    public string? UseAliases { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--volume")]
    public string? Volume { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
