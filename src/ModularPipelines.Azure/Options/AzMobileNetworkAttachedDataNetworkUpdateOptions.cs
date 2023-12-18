using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "attached-data-network", "update")]
public record AzMobileNetworkAttachedDataNetworkUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [CommandSwitch("--adn-name")]
    public string? AdnName { get; set; }

    [CommandSwitch("--data-interface")]
    public string? DataInterface { get; set; }

    [CommandSwitch("--dns-addresses")]
    public string? DnsAddresses { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--napt-configuration")]
    public string? NaptConfiguration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pccp-name")]
    public string? PccpName { get; set; }

    [CommandSwitch("--pcdp-name")]
    public string? PcdpName { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--static-address-pool")]
    public string? StaticAddressPool { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}