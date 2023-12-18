using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb", "server", "create")]
public record AzMariadbServerCreateOptions : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-user")]
    public string? AdminUser { get; set; }

    [BooleanCommandSwitch("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CommandSwitch("--auto-grow")]
    public string? AutoGrow { get; set; }

    [CommandSwitch("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CommandSwitch("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CommandSwitch("--infrastructure-encryption")]
    public string? InfrastructureEncryption { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--public")]
    public string? Public { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sku-name")]
    public string? SkuName { get; set; }

    [CommandSwitch("--ssl-enforcement")]
    public string? SslEnforcement { get; set; }

    [CommandSwitch("--storage-size")]
    public string? StorageSize { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}