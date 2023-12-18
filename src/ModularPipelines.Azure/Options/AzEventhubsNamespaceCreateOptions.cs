using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "create")]
public record AzEventhubsNamespaceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [BooleanCommandSwitch("--enable-kafka")]
    public bool? EnableKafka { get; set; }

    [CommandSwitch("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [BooleanCommandSwitch("--infra-encryption")]
    public bool? InfraEncryption { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--maximum-throughput-units")]
    public string? MaximumThroughputUnits { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--min-tls")]
    public string? MinTls { get; set; }

    [CommandSwitch("--public-network")]
    public string? PublicNetwork { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}