using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "cross-region-lb", "address-pool", "delete")]
public record AzNetworkCrossRegionLbAddressPoolDeleteOptions : AzOptions
{
    [CliOption("--address-pool-name")]
    public string? AddressPoolName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lb-name")]
    public string? LbName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}