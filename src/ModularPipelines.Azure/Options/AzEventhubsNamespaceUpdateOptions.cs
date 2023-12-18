using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "update")]
public record AzEventhubsNamespaceUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--alternate-name")]
    public string? AlternateName { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--cluster-arm-id")]
    public string? ClusterArmId { get; set; }

    [BooleanCommandSwitch("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [BooleanCommandSwitch("--enable-auto-inflate")]
    public bool? EnableAutoInflate { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--endpoint-connections")]
    public string? EndpointConnections { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--kafka-enabled")]
    public bool? KafkaEnabled { get; set; }

    [CommandSwitch("--maximum-throughput-units")]
    public string? MaximumThroughputUnits { get; set; }

    [CommandSwitch("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}

