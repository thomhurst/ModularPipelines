using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("node", "ps")]
[ExcludeFromCodeCoverage]
public record DockerNodePsOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Node { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--no-resolve")]
    public virtual bool? NoResolve { get; set; }

    [CliFlag("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
