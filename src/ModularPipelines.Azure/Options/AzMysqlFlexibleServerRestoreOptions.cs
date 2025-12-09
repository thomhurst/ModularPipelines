using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "restore")]
public record AzMysqlFlexibleServerRestoreOptions(
[property: CliOption("--source-server")] string SourceServer
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliOption("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--public-access")]
    public string? PublicAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--restore-time")]
    public string? RestoreTime { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--storage-auto-grow")]
    public string? StorageAutoGrow { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}