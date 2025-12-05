using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "controller", "create")]
public record AzNetworkfabricControllerCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName
) : AzOptions
{
    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--infra-er-connections")]
    public string? InfraErConnections { get; set; }

    [CliOption("--ipv4-address-space")]
    public string? Ipv4AddressSpace { get; set; }

    [CliOption("--ipv6-address-space")]
    public string? Ipv6AddressSpace { get; set; }

    [CliFlag("--is-workload-management-network-enabled")]
    public bool? IsWorkloadManagementNetworkEnabled { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [CliOption("--nfc-sku")]
    public string? NfcSku { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workload-er-connections")]
    public string? WorkloadErConnections { get; set; }
}