using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "update")]
public record AzEventhubsNamespaceUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--alternate-name")]
    public string? AlternateName { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--cluster-arm-id")]
    public string? ClusterArmId { get; set; }

    [CliFlag("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CliFlag("--enable-auto-inflate")]
    public bool? EnableAutoInflate { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--endpoint-connections")]
    public string? EndpointConnections { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--kafka-enabled")]
    public bool? KafkaEnabled { get; set; }

    [CliOption("--maximum-throughput-units")]
    public string? MaximumThroughputUnits { get; set; }

    [CliOption("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}