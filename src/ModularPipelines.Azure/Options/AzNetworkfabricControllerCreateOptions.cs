using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "controller", "create")]
public record AzNetworkfabricControllerCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--infra-er-connections")]
    public string? InfraErConnections { get; set; }

    [CommandSwitch("--ipv4-address-space")]
    public string? Ipv4AddressSpace { get; set; }

    [CommandSwitch("--ipv6-address-space")]
    public string? Ipv6AddressSpace { get; set; }

    [BooleanCommandSwitch("--is-workload-management-network-enabled")]
    public bool? IsWorkloadManagementNetworkEnabled { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [CommandSwitch("--nfc-sku")]
    public string? NfcSku { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workload-er-connections")]
    public string? WorkloadErConnections { get; set; }
}