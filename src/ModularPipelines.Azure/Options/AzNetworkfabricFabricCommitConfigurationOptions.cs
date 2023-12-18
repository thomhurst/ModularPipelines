using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "fabric", "commit-configuration")]
public record AzNetworkfabricFabricCommitConfigurationOptions(
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
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

