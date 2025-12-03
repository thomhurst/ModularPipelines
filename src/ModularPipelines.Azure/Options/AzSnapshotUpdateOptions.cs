using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snapshot", "update")]
public record AzSnapshotUpdateOptions : AzOptions
{
    [CliFlag("--accelerated-network")]
    public bool? AcceleratedNetwork { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--disk-access")]
    public string? DiskAccess { get; set; }

    [CliOption("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network-access-policy")]
    public string? NetworkAccessPolicy { get; set; }

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
}