using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout")]
[ExcludeFromCodeCoverage]
public record DockerScoutOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Command { get; set; }

    [CliOption("--verbose-debug")]
    public virtual string? VerboseDebug { get; set; }
}
