using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snapshot", "create")]
public record AzSnapshotCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--accelerated-network")]
    public bool? AcceleratedNetwork { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [BooleanCommandSwitch("--copy-start")]
    public bool? CopyStart { get; set; }

    [CommandSwitch("--disk-access")]
    public string? DiskAccess { get; set; }

    [CommandSwitch("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--elastic-san-id")]
    public string? ElasticSanId { get; set; }

    [CommandSwitch("--encryption-type")]
    public string? EncryptionType { get; set; }

    [BooleanCommandSwitch("--for-upload")]
    public bool? ForUpload { get; set; }

    [CommandSwitch("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [BooleanCommandSwitch("--incremental")]
    public bool? Incremental { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--network-access-policy")]
    public string? NetworkAccessPolicy { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--size-gb")]
    public string? SizeGb { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--source-storage-account-id")]
    public int? SourceStorageAccountId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}