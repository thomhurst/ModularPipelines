using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "import", "create")]
public record AzMysqlFlexibleServerImportCreateOptions(
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--data-source-type")] string DataSourceType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-user")]
    public string? AdminUser { get; set; }

    [CommandSwitch("--auto-scale-iops")]
    public string? AutoScaleIops { get; set; }

    [CommandSwitch("--backup-identity")]
    public string? BackupIdentity { get; set; }

    [CommandSwitch("--backup-key")]
    public string? BackupKey { get; set; }

    [CommandSwitch("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CommandSwitch("--data-source-backup-dir")]
    public string? DataSourceBackupDir { get; set; }

    [CommandSwitch("--data-source-sas-token")]
    public string? DataSourceSasToken { get; set; }

    [CommandSwitch("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CommandSwitch("--high-availability")]
    public string? HighAvailability { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--iops")]
    public string? Iops { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CommandSwitch("--public-access")]
    public string? PublicAccess { get; set; }

    [CommandSwitch("--sku-name")]
    public string? SkuName { get; set; }

    [CommandSwitch("--standby-zone")]
    public string? StandbyZone { get; set; }

    [CommandSwitch("--storage-auto-grow")]
    public string? StorageAutoGrow { get; set; }

    [CommandSwitch("--storage-size")]
    public string? StorageSize { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}