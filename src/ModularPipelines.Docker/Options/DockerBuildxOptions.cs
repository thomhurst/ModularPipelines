using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("buildx")]
[ExcludeFromCodeCoverage]
public record DockerBuildxOptions : DockerOptions
{
    [CliOption("--builder")]
    public virtual string? Builder { get; set; }
}
