using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "fabric", "create")]
public record AzNetworkfabricFabricCreateOptions(
[property: CommandSwitch("--fabric-asn")] string FabricAsn,
[property: CommandSwitch("--ipv4-prefix")] string Ipv4Prefix,
[property: CommandSwitch("--managed-network-config")] string ManagedNetworkConfig,
[property: CommandSwitch("--nf-sku")] string NfSku,
[property: CommandSwitch("--nfc-id")] string NfcId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--server-count-per-rack")] int ServerCountPerRack,
[property: CommandSwitch("--ts-config")] string TsConfig
) : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--ipv6-prefix")]
    public string? Ipv6Prefix { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--rack-count")]
    public int? RackCount { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}