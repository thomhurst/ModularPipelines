using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "pull")]
[ExcludeFromCodeCoverage]
public record DockerComposePullOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Service { get; set; }

    [CliOption("--ignore-buildable")]
    public virtual string? IgnoreBuildable { get; set; }

    [CliOption("--ignore-pull-failures")]
    public virtual string? IgnorePullFailures { get; set; }

    [CliOption("--include-deps")]
    public virtual string? IncludeDeps { get; set; }

    [CliFlag("--no-parallel")]
    public virtual bool? NoParallel { get; set; }

    [CliFlag("--parallel")]
    public virtual bool? Parallel { get; set; }

    [CliOption("--policy")]
    public virtual string? Policy { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
