using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "create")]
public record AzNetworkLbCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--frontend-ip-name")]
    public string? FrontendIpName { get; set; }

    [CommandSwitch("--frontend-ip-zone")]
    public string? FrontendIpZone { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CommandSwitch("--public-ip-dns-name")]
    public string? PublicIpDnsName { get; set; }

    [CommandSwitch("--public-ip-zone")]
    public string? PublicIpZone { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandSwitch("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}