using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "attached-data-network", "create")]
public record AzMobileNetworkAttachedDataNetworkCreateOptions(
[property: CliOption("--adn-name")] string AdnName,
[property: CliOption("--data-interface")] string DataInterface,
[property: CliOption("--dns-addresses")] string DnsAddresses,
[property: CliOption("--pccp-name")] string PccpName,
[property: CliOption("--pcdp-name")] string PcdpName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--napt-configuration")]
    public string? NaptConfiguration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--static-address-pool")]
    public string? StaticAddressPool { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}