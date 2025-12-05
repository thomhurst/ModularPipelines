using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "build")]
[ExcludeFromCodeCoverage]
public record DockerComposeBuildOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliOption("--build-arg")]
    public virtual IEnumerable<KeyValue>? BuildArg { get; set; }

    [CliOption("--builder")]
    public virtual string? Builder { get; set; }

    [CliFlag("--compress")]
    public virtual bool? Compress { get; set; }

    [CliFlag("--force-rm")]
    public virtual bool? ForceRm { get; set; }

    [CliOption("--memory")]
    public virtual string? Memory { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliOption("--no-rm")]
    public virtual string? NoRm { get; set; }

    [CliFlag("--parallel")]
    public virtual bool? Parallel { get; set; }

    [CliOption("--progress")]
    public virtual string? Progress { get; set; }

    [CliFlag("--pull")]
    public virtual bool? Pull { get; set; }

    [CliFlag("--push")]
    public virtual bool? Push { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--ssh")]
    public virtual string? Ssh { get; set; }

    [CliOption("--with-dependencies")]
    public virtual string? WithDependencies { get; set; }
}
