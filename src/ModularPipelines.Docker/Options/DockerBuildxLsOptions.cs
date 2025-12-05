using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "ls")]
[ExcludeFromCodeCoverage]
public record DockerBuildxLsOptions : DockerOptions
{
    [CliOption("--builder")]
    public virtual string? Builder { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }
}
