using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "push")]
[ExcludeFromCodeCoverage]
public record DockerComposePushOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Service { get; set; }

    [CliOption("--ignore-push-failures")]
    public virtual string? IgnorePushFailures { get; set; }

    [CliOption("--include-deps")]
    public virtual string? IncludeDeps { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
