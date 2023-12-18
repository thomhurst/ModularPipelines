using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-region-lb", "create")]
public record AzNetworkCrossRegionLbCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-allocation")]
    public string? AddressAllocation { get; set; }

    [CommandSwitch("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CommandSwitch("--frontend-ip-name")]
    public string? FrontendIpName { get; set; }

    [CommandSwitch("--frontend-ip-zone")]
    public string? FrontendIpZone { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--public-ip-dns-name")]
    public string? PublicIpDnsName { get; set; }

    [CommandSwitch("--public-ip-zone")]
    public string? PublicIpZone { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}