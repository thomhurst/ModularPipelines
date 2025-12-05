using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "create")]
public record AzMysqlFlexibleServerCreateOptions : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-user")]
    public string? AdminUser { get; set; }

    [CliOption("--auto-scale-iops")]
    public string? AutoScaleIops { get; set; }

    [CliOption("--backup-identity")]
    public string? BackupIdentity { get; set; }

    [CliOption("--backup-key")]
    public string? BackupKey { get; set; }

    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CliOption("--high-availability")]
    public string? HighAvailability { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--iops")]
    public string? Iops { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--public-access")]
    public string? PublicAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--standby-zone")]
    public string? StandbyZone { get; set; }

    [CliOption("--storage-auto-grow")]
    public string? StorageAutoGrow { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}