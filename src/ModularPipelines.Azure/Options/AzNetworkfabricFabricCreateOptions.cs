using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkfabric", "fabric", "create")]
public record AzNetworkfabricFabricCreateOptions(
[property: CliOption("--fabric-asn")] string FabricAsn,
[property: CliOption("--ipv4-prefix")] string Ipv4Prefix,
[property: CliOption("--managed-network-config")] string ManagedNetworkConfig,
[property: CliOption("--nf-sku")] string NfSku,
[property: CliOption("--nfc-id")] string NfcId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--server-count-per-rack")] int ServerCountPerRack,
[property: CliOption("--ts-config")] string TsConfig
) : AzOptions
{
    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--ipv6-prefix")]
    public string? Ipv6Prefix { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--rack-count")]
    public int? RackCount { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}