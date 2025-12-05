using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "du")]
[ExcludeFromCodeCoverage]
public record DockerBuildxDuOptions : DockerOptions
{
    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--verbose")]
    public virtual string? Verbose { get; set; }
}
