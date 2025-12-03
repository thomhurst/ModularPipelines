using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "integration", "delete")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationDeleteOptions : DockerOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }
}
