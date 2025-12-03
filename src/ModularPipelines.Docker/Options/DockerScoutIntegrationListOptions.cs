using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "integration", "list")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationListOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Integration { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }
}
