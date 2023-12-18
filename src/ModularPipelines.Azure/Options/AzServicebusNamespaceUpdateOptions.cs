using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace", "update")]
public record AzServicebusNamespaceUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--alternate-name")]
    public string? AlternateName { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--connections")]
    public string? Connections { get; set; }

    [BooleanCommandSwitch("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--premium-messaging-partitions")]
    public string? PremiumMessagingPartitions { get; set; }

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
}