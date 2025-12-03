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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Service { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Command { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Args { get; set; }

    [CliFlag("--build")]
    public virtual bool? Build { get; set; }

    [CliOption("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CliOption("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliFlag("--no-TTY")]
    public virtual bool? NoTty { get; set; }

    [CliFlag("--no-deps")]
    public virtual bool? NoDeps { get; set; }

    [CliOption("--publish")]
    public virtual string? Publish { get; set; }

    [CliFlag("--quiet-pull")]
    public virtual bool? QuietPull { get; set; }

    [CliFlag("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [CliFlag("--rm")]
    public virtual bool? Rm { get; set; }

    [CliOption("--service-ports")]
    public virtual string? ServicePorts { get; set; }

    [CliFlag("--tty")]
    public virtual bool? Tty { get; set; }

    [CliOption("--use-aliases")]
    public virtual string? UseAliases { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--volume")]
    public virtual string? Volume { get; set; }

    [CliOption("--workdir")]
    public virtual string? Workdir { get; set; }
}
