using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk", "update")]
public record AzDiskUpdateOptions : AzOptions
{
    [CliFlag("--accelerated-network")]
    public bool? AcceleratedNetwork { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--data-access-auth-mode")]
    public string? DataAccessAuthMode { get; set; }

    [CliOption("--disk-access")]
    public string? DiskAccess { get; set; }

    [CliOption("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CliOption("--disk-iops-read-only")]
    public string? DiskIopsReadOnly { get; set; }

    [CliOption("--disk-iops-read-write")]
    public string? DiskIopsReadWrite { get; set; }

    [CliOption("--disk-mbps-read-only")]
    public string? DiskMbpsReadOnly { get; set; }

    [CliOption("--disk-mbps-read-write")]
    public string? DiskMbpsReadWrite { get; set; }

    [CliFlag("--enable-bursting")]
    public bool? EnableBursting { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--max-shares")]
    public string? MaxShares { get; set; }

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

    [CliOption("--size-gb")]
    public string? SizeGb { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}