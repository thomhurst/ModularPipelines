using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "cluster", "create")]
public record AzVmwareClusterCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--cluster-size")]
    public string? ClusterSize { get; set; }

    [CliOption("--hosts")]
    public string? Hosts { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}