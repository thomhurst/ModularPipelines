using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("trust", "sign")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignOptions : DockerOptions
{
    [CliOption("--local")]
    public virtual string? Local { get; set; }
}
