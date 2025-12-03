using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "workload-network", "port-mirroring", "create")]
public record AzVmwareWorkloadNetworkPortMirroringCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}