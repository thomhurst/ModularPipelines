using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "version")]
[ExcludeFromCodeCoverage]
public record DockerBuildxVersionOptions : DockerOptions
{
    [CliOption("--builder")]
    public virtual string? Builder { get; set; }
}
