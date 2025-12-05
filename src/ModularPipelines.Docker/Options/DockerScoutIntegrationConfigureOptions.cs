using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "integration", "configure")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationConfigureOptions : DockerOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--parameter")]
    public virtual string? Parameter { get; set; }
}
