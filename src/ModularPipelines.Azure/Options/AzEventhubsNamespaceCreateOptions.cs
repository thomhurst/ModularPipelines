using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "create")]
public record AzEventhubsNamespaceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CliFlag("--enable-kafka")]
    public bool? EnableKafka { get; set; }

    [CliOption("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [CliFlag("--infra-encryption")]
    public bool? InfraEncryption { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--maximum-throughput-units")]
    public string? MaximumThroughputUnits { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--min-tls")]
    public string? MinTls { get; set; }

    [CliOption("--public-network")]
    public string? PublicNetwork { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}