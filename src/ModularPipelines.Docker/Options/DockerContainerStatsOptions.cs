using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("container", "stats")]
[ExcludeFromCodeCoverage]
public record DockerContainerStatsOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Container { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--no-stream")]
    public virtual string? NoStream { get; set; }

    [CliFlag("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }
}
