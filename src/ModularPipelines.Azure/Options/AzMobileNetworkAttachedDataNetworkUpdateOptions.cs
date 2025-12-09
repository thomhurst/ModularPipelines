using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "attached-data-network", "update")]
public record AzMobileNetworkAttachedDataNetworkUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--adn-name")]
    public string? AdnName { get; set; }

    [CliOption("--data-interface")]
    public string? DataInterface { get; set; }

    [CliOption("--dns-addresses")]
    public string? DnsAddresses { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--napt-configuration")]
    public string? NaptConfiguration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pccp-name")]
    public string? PccpName { get; set; }

    [CliOption("--pcdp-name")]
    public string? PcdpName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--static-address-pool")]
    public string? StaticAddressPool { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}