using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "logs")]
[ExcludeFromCodeCoverage]
public record DockerComposeLogsOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Service { get; set; }

    [CliFlag("--follow")]
    public virtual bool? Follow { get; set; }

    [CliOption("--index")]
    public virtual string? Index { get; set; }

    [CliFlag("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CliOption("--no-log-prefix")]
    public virtual string? NoLogPrefix { get; set; }

    [CliOption("--since")]
    public virtual string? Since { get; set; }

    [CliOption("--tail")]
    public virtual string? Tail { get; set; }

    [CliFlag("--timestamps")]
    public virtual bool? Timestamps { get; set; }

    [CliOption("--until")]
    public virtual string? Until { get; set; }
}
