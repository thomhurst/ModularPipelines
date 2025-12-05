using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "rm")]
[ExcludeFromCodeCoverage]
public record DockerBuildxRmOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Name { get; set; }

    [CliOption("--all-inactive")]
    public virtual string? AllInactive { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--keep-daemon")]
    public virtual string? KeepDaemon { get; set; }

    [CliOption("--keep-state")]
    public virtual string? KeepState { get; set; }
}
