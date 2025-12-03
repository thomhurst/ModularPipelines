using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "workload-network", "port-mirroring", "update")]
public record AzVmwareWorkloadNetworkPortMirroringUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}