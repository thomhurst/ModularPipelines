using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "fabric", "update")]
public record AzNetworkfabricFabricUpdateOptions : AzOptions
{
    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--fabric-asn")]
    public string? FabricAsn { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ipv4-prefix")]
    public string? Ipv4Prefix { get; set; }

    [CliOption("--ipv6-prefix")]
    public string? Ipv6Prefix { get; set; }

    [CliOption("--managed-network-config")]
    public string? ManagedNetworkConfig { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--rack-count")]
    public int? RackCount { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--server-count-per-rack")]
    public int? ServerCountPerRack { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--ts-config")]
    public string? TsConfig { get; set; }
}