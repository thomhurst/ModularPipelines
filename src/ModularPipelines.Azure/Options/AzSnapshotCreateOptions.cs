using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snapshot", "create")]
public record AzSnapshotCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--accelerated-network")]
    public bool? AcceleratedNetwork { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliFlag("--copy-start")]
    public bool? CopyStart { get; set; }

    [CliOption("--disk-access")]
    public string? DiskAccess { get; set; }

    [CliOption("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--elastic-san-id")]
    public string? ElasticSanId { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliFlag("--for-upload")]
    public bool? ForUpload { get; set; }

    [CliOption("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [CliFlag("--incremental")]
    public bool? Incremental { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--network-access-policy")]
    public string? NetworkAccessPolicy { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--size-gb")]
    public string? SizeGb { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--source-storage-account-id")]
    public int? SourceStorageAccountId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}