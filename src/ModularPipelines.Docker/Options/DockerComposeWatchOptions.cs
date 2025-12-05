using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "watch")]
[ExcludeFromCodeCoverage]
public record DockerComposeWatchOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliOption("--no-up")]
    public virtual string? NoUp { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
