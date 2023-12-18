using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "attached-data-network", "create")]
public record AzMobileNetworkAttachedDataNetworkCreateOptions(
[property: CommandSwitch("--adn-name")] string AdnName,
[property: CommandSwitch("--data-interface")] string DataInterface,
[property: CommandSwitch("--dns-addresses")] string DnsAddresses,
[property: CommandSwitch("--pccp-name")] string PccpName,
[property: CommandSwitch("--pcdp-name")] string PcdpName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--napt-configuration")]
    public string? NaptConfiguration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--static-address-pool")]
    public string? StaticAddressPool { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}